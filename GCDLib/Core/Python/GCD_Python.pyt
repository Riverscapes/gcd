import os, random, glob, shutil, datetime
import arcpy


class Toolbox(object):
    def __init__(self):
        """Define the toolbox (the name of the toolbox is the name of the
        .pyt file)."""
        self.label = "GCD Python"
        self.alias = "gcdPython"

        # List of tool classes associated with this toolbox
        self.tools = [BootstrapRoughness, ExtractRasterByMask]


class BootstrapRoughness(object):
    def __init__(self):
        """Define the tool (tool name is the name of the class)."""
        self.label = "Bootstrap Roughness"
        self.description = "Calculates surface roughness through a bootstrapping"
        self.canRunInBackground = False

    def getParameterInfo(self):
        """Define parameter definitions"""
        param0 = arcpy.Parameter(displayName = 'Input Point Feature Class',
                                 name = 'inPointFC',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param1 = arcpy.Parameter(displayName = 'Survey Extent Polygon Feature Class',
                                 name = 'SurveyPolygonFC',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param2 = arcpy.Parameter(displayName = 'In Channel Polygon Feature Class',
                                 name = 'inChannelPolygonFC',
                                 datatype = 'GPString',
                                 parameterType = 'Optional',
                                 direction = 'Input')

        param3 = arcpy.Parameter(displayName ='Input DEM',
                                 name = 'inDEM',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param4 = arcpy.Parameter(displayName ='Number of Iterations',
                                 name = 'iIterations',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param5 = arcpy.Parameter(displayName ='Percent Data Withheld',
                                 name = 'dPercentData',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param6 = arcpy.Parameter(displayName = 'Cell Size',
                                 name = 'cellSize',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')
        
        param7 = arcpy.Parameter(displayName = 'Temporary Workspace',
                                 name = 'tempWorkspace',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param8 = arcpy.Parameter(displayName = 'Output Roughness Raster',
                                 name = 'outRaster',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')
        
        param9 = arcpy.Parameter(displayName = 'Input Vertical Units of Raster',
                                 name = 'eUnits',
                                 datatype = 'GPString',
                                 parameterType = 'Optional',
                                 direction = 'Input')

        params = [param0, param1, param2, param3, param4, param5, param6, param7, param8, param9]
        return params

    def isLicensed(self):
        """Set whether tool is licensed to execute."""
        return True

    def updateParameters(self, parameters):
        """Modify the values and properties of parameters before internal
        validation is performed.  This method is called whenever a parameter
        has been changed."""
        return

    def updateMessages(self, parameters):
        """Modify the messages created by internal validation for each tool
        parameter.  This method is called after internal validation."""
        return

    def execute(self, parameters, messages):
        """The source code of the tool."""
        if arcpy.CheckExtension('3D') == 'Available':
            arcpy.CheckOutExtension('3D')
            arcpy.AddMessage('\n3D Analyst extension checked out.')
        else:
            arcpy.AddMessage('\n3D Analyst extension not available.')
            sys.exit()

        if arcpy.CheckExtension('Spatial') == 'Available':
            arcpy.CheckOutExtension('Spatial')
            arcpy.AddMessage('\nSpatial Analyst extension checked out.')
        else:
            arcpy.AddMessage('\nSpatial Analyst extension not available.')
            sys.exit()
        
        inPtsFC = parameters[0].valueAsText
        inSurveyExtentFC = parameters[1].valueAsText
        inChannelFC = parameters[2].valueAsText
        inDEM = parameters[3].valueAsText
        iterations = int(parameters[4].valueAsText)
        percentData = float(parameters[5].valueAsText)
        cellSize = float(parameters[6].valueAsText)
        tempWorkspace = parameters[7].valueAsText
        outRaster = parameters[8].valueAsText
        eUnits = parameters[9].valueAsText

        arcpy.env.overwrite = True
        timeStamp = datetime.datetime.now().strftime('%H%M%S')
        tempDir = os.path.join(tempWorkspace, 'Bootstrapping_' + timeStamp)
        os.makedirs(tempDir)

        outChannelFilePath = os.path.join(tempDir, "outChannel.shp")
        ##########################
        #Workflow

        #Get subset of points within selected area of polygon
        ptsLyr = 'tempPtLayer'
        extentLyr = 'extentLayer'
        inChannelLyr = 'tempInChannelLyr'
        arcpy.MakeFeatureLayer_management(inPtsFC, ptsLyr)
        arcpy.MakeFeatureLayer_management(inSurveyExtentFC, extentLyr)
        if inChannelFC:
            arcpy.MakeFeatureLayer_management(inChannelFC, inChannelLyr)
            arcpy.AddMessage("Using channel extent polygon.")
        else:
            arcpy.AddMessage("Not using channel extent polygon.")


        #Make a subset of the edge points to always be added to the subset otherwise the output is smaller than the input
        arcpy.SelectLayerByLocation_management(ptsLyr, 'BOUNDARY_TOUCHES', extentLyr)
        boundaryArray = arcpy.Array()

        fields = ['SHAPE@X', 'SHAPE@Y', 'SHAPE@Z']
        with arcpy.da.SearchCursor(ptsLyr, fields) as rows:
            for row in rows:
                pt = arcpy.Point(row[0], row[1], row[2])
                boundaryArray.add(pt)

        arcpy.SelectLayerByLocation_management(ptsLyr, 'INTERSECT', extentLyr)

        if inChannelFC:
            arcpy.SelectLayerByLocation_management(ptsLyr, 'COMPLETELY_WITHIN', inChannelLyr, selection_type = 'REMOVE_FROM_SELECTION')
            outChannelLyr = 'tempOutChannelLyr'
            arcpy.Erase_analysis(inSurveyExtentFC, inChannelFC, outChannelFilePath)
            arcpy.MakeFeatureLayer_management(outChannelFilePath, outChannelLyr)
            #remove boundary points from the selection because they will always be included
            arcpy.SelectLayerByLocation_management(ptsLyr, 'BOUNDARY_TOUCHES', extentLyr, selection_type = 'REMOVE_FROM_SELECTION')

        if inChannelFC:
            tinExtent = outChannelLyr
        else:
            tinExtent = extentLyr   

        referenceRas = arcpy.Raster(inDEM)
        spatialRef = referenceRas.spatialReference
        arcpy.env.extent = referenceRas.extent

        #Loop over points of subset and created a temporary feature class of random points
        tempArray = arcpy.Array()

        fields = ['SHAPE@X', 'SHAPE@Y', 'SHAPE@Z']
        with arcpy.da.SearchCursor(ptsLyr, fields) as rows:
            for row in rows:
                pt = arcpy.Point(row[0], row[1], row[2])
                tempArray.add(pt)

        try:
            ct = 1
            while ct <= iterations:

                #Get 80% of tempArray
                subsetNum = tempArray.count * percentData
                #subsetArray = arcpy.Array()
                subsetArray = boundaryArray
                for i in xrange(int(subsetNum)):
                    subsetArray.add(random.choice(tempArray))

                #ptGeometry = arcpy.Geometry('POINT', subsetArray, spatialRef, True)
                #ptGeometry = arcpy.PointGeometry(subsetArray, spatialRef, True)

                tempFc = arcpy.CreateFeatureclass_management('in_memory', 'tempFC', 'POINT', ptsLyr, 'DISABLED', 'ENABLED', spatialRef)

                insertCur = arcpy.da.InsertCursor(tempFc, ['SHAPE@X', 'SHAPE@Y', 'SHAPE@Z'])
                for i in xrange(subsetArray.count):
                    data = (subsetArray[i].X, subsetArray[i].Y, subsetArray[i].Z)
                    insertCur.insertRow(data)

                #Create a TIN and DEM from the set of points
                sTinName = 'tin%s'%ct
                sTin_Path = os.path.join(tempDir, sTinName)
                arcpy.CreateTin_3d(sTin_Path,
                                   spatialRef,[['in_memory\\tempFC', 'Shape.Z', 'Mass_Points', '<None>'],[tinExtent, '<None>', 'Soft_Clip', '<None>']], "DELAUNAY")#Note adding a tag field for the extent polygon helps to extend the reach of the output raster

                #Create DEM
                sDEM_Name = 'temp%s.tif'%ct
                sDEM_Path = os.path.join(tempDir, sDEM_Name)
                arcpy.TinRaster_3d(sTin_Path, sDEM_Path, 'FLOAT', 'NATURAL_NEIGHBORS', 'CELLSIZE ' + str(cellSize))


                #Difference in Raster operation to create temp raster
                tempDEM = arcpy.Raster(sDEM_Path)
                arcpy.AddMessage("Mean {0}".format(str(tempDEM.mean)))

                diffRas = abs(referenceRas - tempDEM)
                if ct == 1:
                    #BootstrapResults = arcpy.RasterToNumPyArray(diffRas)
                    BootstrapResults = diffRas
                    print "Run %s" % ct
                    ct += 1
                    arcpy.Delete_management('in_memory\\tempFC')
                    continue
                else:
                    #tempDiffArray = arcpy.RasterToNumPyArray(diffRas)
                    #diffArray = (diffArray + tempDiffArray)/ct
                    BootstrapResults += diffRas
                    print "Run %s" % ct
                    ct += 1
                    arcpy.Delete_management('in_memory\\tempFC')
                    continue

            arcpy.env.overwrite = True
            arcpy.env.extent = referenceRas.extent
            #diffRas = arcpy.NumPyArrayToRaster(diffArray, referenceRas.extent.lowerLeft, referenceRas.meanCellWidth, referenceRas.meanCellHeight, referenceRas.noDataValue)
            #diffRas.save(r'C:\Users\A01674762\Desktop\ScratchWorkspace\GCD\BootstrappingDEM\BootstrapResults.tif')
            if eUnits == 'mm':
                BootstrapResults = (BootstrapResults/ct)
            elif eUnits == 'cm':
                BootstrapResults = (BootstrapResults/ct) * 10
            elif eUnits == 'm':
                BootstrapResults = (BootstrapResults/ct) * (1000)
            elif eUnits == 'km':
                BootstrapResults = (BootstrapResults/ct) * (1000000)
            elif eUnits == 'inch':
                BootstrapResults = (BootstrapResults/ct) * (25.4)
            elif eUnits == 'ft':
                BootstrapResults = (BootstrapResults/ct) * (304.8)
            elif eUnits == 'yard':
                BootstrapResults = (BootstrapResults/ct) * (914.4)
            elif eUnits == 'mile':
                BootstrapResults = (BootstrapResults/ct) * (1609300)
            else:
                BootstrapResults = (BootstrapResults/ct)
                
            BootstrapResults.save(outRaster)
        except:
            print sys.exc_info()[1]
            return
        finally:
            arcpy.Delete_management(sDEM_Path)
            try:
                shutil.rmtree(tempDir)
            except:
                return


class ExtractRasterByMask(object):
    def __init__(self):
        """Define the tool (tool name is the name of the class)."""
        self.label = "Extract Raster By Mask"
        self.description = ""
        self.canRunInBackground = False

    def getParameterInfo(self):
        """Define parameter definitions"""
        param0 = arcpy.Parameter(displayName = 'Input Raster',
                                 name = 'inRaster',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param1 = arcpy.Parameter(displayName = 'Channel Unit Polygon Feature Class',
                                 name = 'channelUnitPolygonFC',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param2 = arcpy.Parameter(displayName = 'Channel Unit Field Name',
                                 name = 'channelUnitFieldName',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')
        
        param3 = arcpy.Parameter(displayName ='Channel Unit Value',
                                 name = 'channelUnitValue',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        param4 = arcpy.Parameter(displayName = 'Output Roughness Raster',
                                 name = 'outRaster',
                                 datatype = 'GPString',
                                 parameterType = 'Required',
                                 direction = 'Input')

        params = [param0, param1, param2, param3, param4]
        return params

    def isLicensed(self):
        """Set whether tool is licensed to execute."""
        return True

    def updateParameters(self, parameters):
        """Modify the values and properties of parameters before internal
        validation is performed.  This method is called whenever a parameter
        has been changed."""
        return

    def updateMessages(self, parameters):
        """Modify the messages created by internal validation for each tool
        parameter.  This method is called after internal validation."""
        return

    def execute(self, parameters, messages):
        """The source code of the tool."""

        if arcpy.CheckExtension('Spatial') == 'Available':
            arcpy.CheckOutExtension('Spatial')
            arcpy.AddMessage('\nSpatial Analyst extension checked out.')
        else:
            arcpy.AddMessage('\nSpatial Analyst extension not available.')
            sys.exit()
        
        inRaster = parameters[0].valueAsText
        inChannelUnitsFC = parameters[1].valueAsText
        inChannelFieldName = parameters[2].valueAsText
        channelUnitValue = parameters[3].valueAsText
        outRaster = parameters[4].valueAsText

        arcpy.env.overwrite = True

        ##########################
        #Workflow

        #Get subset of points within selected area of polygon
        try:
            spatialRef = arcpy.Describe(arcpy.Raster(inRaster)).SpatialReference

            extentLyr = 'extentLayer'
            arcpy.MakeFeatureLayer_management(inChannelUnitsFC, extentLyr)
            selectStatement = '"{0}" = {1}'.format(inChannelFieldName, channelUnitValue)
            arcpy.AddMessage(selectStatement)
            arcpy.SelectLayerByAttribute_management(extentLyr, 'NEW_SELECTION', selectStatement)
            #arcpy.gp.ExtractByMask_sa(inRaster, extentLyr, outRaster)


            with arcpy.da.SearchCursor(extentLyr, ['SHAPE@']) as sCur:
                for row in sCur:
                    ptArray = arcpy.Array(row[0].getPart(0))
                    geom = arcpy.Geometry('polygon', ptArray, spatialRef)
                    processingExtent = arcpy.Extent(geom.extent.XMin - 50, geom.extent.YMin - 50, geom.extent.XMax + 50, geom.extent.YMax + 50)
                    tempRas = 'TempRas'
                    arcpy.MakeRasterLayer_management(inRaster, tempRas, envelope = processingExtent)
                    outRas = arcpy.sa.ExtractByMask(tempRas, extentLyr)
                    outRas.save(outRaster)
                    arcpy.Delete_management(tempRas)

        except:
            print sys.exc_info()[1]
            return
