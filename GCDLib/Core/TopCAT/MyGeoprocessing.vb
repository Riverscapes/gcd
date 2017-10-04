
Namespace TopCAT


    Public Class MyGeoprocessing

        Public Shared Sub RunToolGeoprocessingTool(ByVal toolName As String,
                                                   ByVal geoprocessorEngine As ESRI.ArcGIS.Geoprocessing.GeoProcessor,
                                                   ByVal parameterHolder As ESRI.ArcGIS.esriSystem.IVariantArray,
                                                   Optional ByVal suppressGeoprocessingMessages As Boolean = False,
                                                   Optional ByVal trackCancel As ESRI.ArcGIS.esriSystem.ITrackCancel = Nothing)

            'Store user's current add to map to be reset later, but set our add to map to false
            Dim bAddtoMap As Boolean = geoprocessorEngine.AddOutputsToMap

            ' Set the overwrite output option to true
            geoprocessorEngine.OverwriteOutput = True

            Try
                Dim aoInitialize As ESRI.ArcGIS.esriSystem.IAoInitialize = New ESRI.ArcGIS.esriSystem.AoInitializeClass
                Dim productCode = aoInitialize.InitializedProduct
                aoInitialize.Initialize(productCode)

                geoprocessorEngine.AddOutputsToMap = False

                geoprocessorEngine.Execute(toolName, parameterHolder, trackCancel)
                If suppressGeoprocessingMessages = False Then
                    MyGeoprocessing.ReturnMessages(geoprocessorEngine)
                End If

                aoInitialize.Shutdown()
                aoInitialize = Nothing

            Catch err As Exception
                MsgBox(err.Message, MsgBoxStyle.OkOnly, "Exception")

                MyGeoprocessing.ReturnMessages(geoprocessorEngine)

            Finally
                geoprocessorEngine.AddOutputsToMap = bAddtoMap
            End Try
        End Sub


        ' Function for returning the tool messages.
        Public Shared Sub ReturnMessages(ByVal geoprocessingEngine As ESRI.ArcGIS.Geoprocessing.GeoProcessor)
            ' Print out the messages from tool executions
            Dim Count As Integer
            If geoprocessingEngine.MessageCount > 0 Then
                Dim messageBuilder As New System.Text.StringBuilder
                For Count = 0 To geoprocessingEngine.MessageCount - 1
                    messageBuilder.Append(geoprocessingEngine.GetMessage(Count)).AppendLine().AppendLine()
                Next
                MsgBox(messageBuilder.ToString(), MsgBoxStyle.OkOnly, "Geoprocessing Messages")
            End If
        End Sub

    End Class

End Namespace