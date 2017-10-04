Imports System.Linq

Namespace Core.TopCAT
    Public Class Stats

        Public Structure UncertaintyAnalysis
            Public X As List(Of Double)
            Public Y As List(Of Double)
            Public Uncertainty As List(Of Single)
            Public Param1 As Parameter
            Public Param2 As Parameter

            Public Structure Parameter
                Public Values As List(Of Single)
                Public ClassifiedValues As List(Of UShort)


                Public Sub CreateCrispGroups(ByVal medianParam As Single, ByVal stDevParam As Single)

                    Dim ClassifiedValues As New List(Of UShort)
                    Me.ClassifiedValues = ClassifiedValues

                    For Each value In Me.Values
                        Select Case value

                            Case Is <= medianParam
                                Me.ClassifiedValues.Add(1)
                            Case medianParam To medianParam + (2 * stDevParam)
                                Me.ClassifiedValues.Add(2)
                            Case Is > medianParam + (2 * stDevParam)
                                Me.ClassifiedValues.Add(3)

                        End Select
                    Next

                End Sub

            End Structure

            Sub addValues(ByVal addX As Double, ByVal addY As Double, ByVal addUncertainty As Single, ByVal addParam1 As Single, ByVal addParam2 As Single)
                Me.X.Add(addX)
                Me.Y.Add(addY)
                Me.Uncertainty.Add(addUncertainty)
                Me.Param1.Values.Add(addParam1)
                Me.Param2.Values.Add(addParam2)
            End Sub

            'Sub classifyParameter1(ByVal paramClass As UShort)

            '    Me.Param1Class.Add(paramClass)

            'End Sub

            'Sub classifyParameter2(ByVal paramClass As UShort)

            '    Me.Param2Class.Add(paramClass)

            'End Sub

            Public Sub ReadFromCSV(ByVal pFilePath As String, ByVal seperator As String)

                Dim X As Double = Nothing
                Dim Y As Double = Nothing
                Dim Uncertainty As Single = Nothing
                Dim Param1 As Single = Nothing
                Dim Param2 As Single = Nothing

                Using streamReader As New System.IO.StreamReader(pFilePath)
                    streamReader.ReadLine()
                    Do While (streamReader.Peek() > -1)
                        Dim newLine As String = streamReader.ReadLine()
                        X = System.Double.Parse(newLine.Split(seperator)(0))
                        Y = System.Double.Parse(newLine.Split(seperator)(1))
                        Uncertainty = System.Single.Parse(newLine.Split(seperator)(2))
                        Param1 = System.Single.Parse(newLine.Split(seperator)(3))
                        Param2 = System.Single.Parse(newLine.Split(seperator)(4))
                        addValues(X, Y, Uncertainty, Param1, Param2)
                    Loop
                End Using

            End Sub

        End Structure

        Public Structure LinearRegressionResults

            Public Slope As Single
            Public Intercept As Single
            Public Tscore As Single
            Public Probability As Single
            Public DegreesOfFreedom As Single

        End Structure

        'TODO CREATE DATA STRUCTURE FOR ANOVA TABLE
        Public Structure ANOVA_Table

            Dim SSM As Single
            Dim SSE As Single
            Dim SST As Single
            Dim MSM As Single
            Dim MSE As Single
            Dim MST As Single
            Dim DegreesOfFreedom As UInteger
            Dim fScore As Single

        End Structure

        Public Structure TupleMeanAndCount
            Public Mean As Single
            Public Count As Integer

            Public Sub addValues(ByVal inputMean As Single, ByVal inputCount As Integer)
                Me.Mean = inputMean
                Me.Count = inputCount
            End Sub

        End Structure

        Public Shared Function GetMedian(ByVal valueList As List(Of Double)) As Single

            Dim median As Single = Nothing
            Dim copyList = New List(Of Double)(valueList)
            copyList.Sort()
            If copyList.Count Mod 2 <> 0 Then
                Dim middle As UInteger = copyList.Count / 2
                median = copyList.Item(middle)
            ElseIf copyList.Count Mod 2 = 0 Then
                median = copyList.Item(copyList.Count / 2)
            End If

            copyList = Nothing
            Return median

        End Function

        Public Shared Function GetMedian(ByVal valueList As List(Of Single)) As Single

            Dim median As Single = Nothing
            Dim copyList = New List(Of Single)(valueList)
            copyList.Sort()
            If copyList.Count Mod 2 <> 0 Then
                Dim middle As Single = copyList.Count / 2.0
                median = (copyList.Item(middle - 0.5) + copyList.Item(middle + 0.5)) / 2
            ElseIf copyList.Count Mod 2 = 0 Then
                median = copyList.Item(copyList.Count / 2)
            End If

            copyList = Nothing
            Return median

        End Function

        Public Shared Function Get1stQuarter(ByVal valueList As List(Of Single)) As Single

            Dim firstQuarter As Single = Nothing
            Dim copyList = New List(Of Single)(valueList)
            copyList.Sort()
            Dim medianN As Single = copyList.Count / 2
            Dim firstQuarterList As List(Of Single) = copyList.GetRange(0, medianN)
            If firstQuarterList.Count Mod 2 <> 0 Then
                Dim middle As Single = firstQuarterList.Count / 2.0
                firstQuarter = (firstQuarterList.Item(middle - 0.5) + firstQuarterList.Item(middle + 0.5)) / 2
            ElseIf firstQuarterList.Count Mod 2 = 0 Then
                firstQuarter = firstQuarterList.Item(firstQuarterList.Count / 2)
            End If
            firstQuarter = firstQuarterList.Item(firstQuarterList.Count / 2)
            copyList = Nothing
            firstQuarterList = Nothing
            Return firstQuarter

        End Function

        Public Shared Function Get1stQuarter(ByVal valueList As List(Of Double)) As Single

            Dim firstQuarter As Single = Nothing
            Dim copyList = New List(Of Double)(valueList)
            copyList.Sort()
            Dim medianN As Single = copyList.Count / 2
            Dim firstQuarterList As List(Of Double) = copyList.GetRange(0, medianN)
            If firstQuarterList.Count Mod 2 <> 0 Then
                Dim middle As Single = firstQuarterList.Count / 2.0
                firstQuarter = (firstQuarterList.Item(middle - 0.5) + firstQuarterList.Item(middle + 0.5)) / 2
            ElseIf firstQuarterList.Count Mod 2 = 0 Then
                firstQuarter = firstQuarterList.Item(firstQuarterList.Count / 2)
            End If
            firstQuarter = firstQuarterList.Item(firstQuarterList.Count / 2)
            copyList = Nothing
            firstQuarterList = Nothing
            Return firstQuarter

        End Function

        Public Shared Function Get3rdQuarter(ByVal valueList As List(Of Single)) As Single

            Dim thirdQuarter As Single = Nothing
            Dim copyList = New List(Of Single)(valueList)
            copyList.Sort()
            Dim medianN As Integer = Math.Round(copyList.Count / 2)
            Dim thirdQuarterList As List(Of Single) = copyList.GetRange(medianN - 1, (copyList.Count - 1) - medianN)
            If thirdQuarterList.Count Mod 2 <> 0 Then
                Dim middle As Single = thirdQuarterList.Count / 2.0
                thirdQuarter = (thirdQuarterList.Item(middle - 0.5) + thirdQuarterList.Item(middle + 0.5)) / 2
            ElseIf thirdQuarterList.Count Mod 2 = 0 Then
                thirdQuarter = thirdQuarterList.Item(thirdQuarterList.Count / 2)
            End If
            copyList = Nothing
            thirdQuarterList = Nothing
            Return thirdQuarter

        End Function

        Public Shared Function Get3rdQuarter(ByVal valueList As List(Of Double)) As Single

            Dim thirdQuarter As Single = Nothing
            Dim copyList = New List(Of Double)(valueList)
            copyList.Sort()
            Dim medianN As Integer = Math.Round(copyList.Count / 2)
            Dim thirdQuarterList As List(Of Double) = copyList.GetRange(medianN - 1, (copyList.Count - 1) - medianN)
            If thirdQuarterList.Count Mod 2 <> 0 Then
                Dim middle As Single = thirdQuarterList.Count / 2.0
                thirdQuarter = (thirdQuarterList.Item(middle - 0.5) + thirdQuarterList.Item(middle + 0.5)) / 2
            ElseIf thirdQuarterList.Count Mod 2 = 0 Then
                thirdQuarter = thirdQuarterList.Item(thirdQuarterList.Count / 2)
            End If
            copyList = Nothing
            thirdQuarterList = Nothing
            Return thirdQuarter

        End Function

        Public Shared Function GetStandardDeviation(ByVal valueList As List(Of Single)) As Single

            'Dim sumOfSquares As Single = GetSumOfSquares(valueList)
            Dim devianceFromMean As New List(Of Single)
            Dim sampleMean As Single = valueList.Average
            For Each value In valueList
                devianceFromMean.Add(Math.Pow(value - sampleMean, 2))
            Next

            Dim stDev As Single = Math.Sqrt(devianceFromMean.Sum / (valueList.Count - 1))

            'Dim squaredSum As Single = Math.Pow(valueList.Sum, 2.0)

            'Dim stDev As Single = Math.Sqrt((sumOfSquares - (squaredSum / valueList.Count)) / (valueList.Count - 1))
            Return stDev

        End Function

        Public Shared Function GetStandardDeviation(ByVal valueList As List(Of Double)) As Single


            'Dim sumOfSquares As Single = GetSumOfSquares(valueList)
            Dim devianceFromMean As New List(Of Single)
            Dim sampleMean As Single = valueList.Average
            For Each value In valueList
                devianceFromMean.Add(Math.Pow(value - sampleMean, 2))
            Next

            Dim stDev As Single = Math.Sqrt(devianceFromMean.Sum / (valueList.Count - 1))

            'Dim squaredSum As Single = Math.Pow(valueList.Sum, 2.0)

            'Dim stDev As Single = Math.Sqrt((sumOfSquares - (squaredSum / valueList.Count)) / (valueList.Count - 1))
            Return stDev

        End Function

        Public Shared Function GetSumOfSquares(ByVal valueList As List(Of Single)) As Single

            Dim squaredList As List(Of Single) = New List(Of Single)
            For Each i In valueList
                squaredList.Add(Math.Pow(i, 2.0))
            Next

            Dim sumOfSquares As Single = squaredList.Sum()
            Return sumOfSquares

        End Function

        Public Shared Function GetSumOfSquares(ByVal valueList As List(Of Double)) As Single

            Dim squaredList As List(Of Double) = New List(Of Double)
            For Each i In valueList
                squaredList.Add(Math.Pow(i, 2.0))
            Next

            Dim sumOfSquares As Single = squaredList.Sum()
            Return sumOfSquares

        End Function

        Public Shared Function MultiplyTwoLists(ByVal paramList1 As List(Of Single), ByVal paramList2 As List(Of Single)) As List(Of Single)

            Dim resultsList As New List(Of Single)
            For i As Integer = 0 To paramList1.Count - 1

                resultsList.Add(paramList1.Item(i) * paramList2.Item(i))

            Next

            Return resultsList

        End Function

        Public Shared Function MultiplyTwoLists(ByVal paramList1 As List(Of Double), ByVal paramList2 As List(Of Double)) As List(Of Double)

            Dim resultsList As New List(Of Double)
            For i As Integer = 0 To paramList1.Count - 1

                resultsList.Add(paramList1.Item(i) * paramList2.Item(i))

            Next

            Return resultsList

        End Function

        Public Shared Function CalculateANOVA_Table(ByVal xValueList As List(Of Single),
                                                    ByVal yValueList As List(Of Single),
                                                    ByVal Slope As Single,
                                                    ByVal Intercept As Single) As Single

            Dim fScore As Single = Nothing
            Dim n As Integer = xValueList.Count
            Dim xMean As Single = xValueList.Sum / n
            Dim yMean As Single = yValueList.Sum / n
            Dim ssmYList As New List(Of Single)
            Dim sseYList As New List(Of Single)
            For i As Integer = 0 To n - 1

                Dim predictedY = CalculatePredictedY(Slope, Intercept, xValueList.Item(i))
                ssmYList.Add(Math.Pow(predictedY - yMean, 2))
                sseYList.Add(Math.Pow(yValueList.Item(i) - predictedY, 2))
                'yTermList.Add(yValueList.Item(i) - predictedY)
                'xTermList.Add(xValueList.Item(i) - xMean)

            Next

            Dim SSM As Single = ssmYList.Sum
            Dim SSE As Single = sseYList.Sum
            Dim SST As Single = SSE
            Dim MSM As Single = SSM / 1
            Dim MSE As Single = SSE / (n - 2)
            Dim MST As Single = SST / (n - 1)
            fScore = MSM / MSE
            Return fScore

        End Function

        Public Shared Function CalculateStandardErrorOfSlope(ByVal xValueList As List(Of Single),
                                                             ByVal yValueList As List(Of Single),
                                                             ByVal Slope As Single,
                                                             ByVal Intercept As Single) As Single

            Dim standardErrorOfSlope As Single = Nothing
            Dim n As Integer = xValueList.Count
            Dim xMean As Single = xValueList.Sum / n
            Dim yMean As Single = yValueList.Sum / n
            Dim yTermList As New List(Of Single)
            Dim xTermList As New List(Of Single)

            For i As Integer = 0 To n - 1

                Dim predictedY = CalculatePredictedY(Slope, Intercept, xValueList.Item(i))
                yTermList.Add(Math.Pow(yValueList.Item(i) - predictedY, 2))
                xTermList.Add(Math.Pow(xValueList.Item(i) - xMean, 2))

            Next

            Dim standardErrorNumerator As Single = Math.Sqrt(yTermList.Sum / (n - 2))
            standardErrorOfSlope = standardErrorNumerator
            Dim standardErrorDenomenator As Single = Math.Sqrt(xTermList.Sum)
            standardErrorOfSlope = standardErrorNumerator / standardErrorDenomenator

            Return standardErrorOfSlope

        End Function

        Public Shared Function CalculatePredictedY(ByVal Slope As Single, ByVal Intercept As Single, ByVal xValue As Single) As Single

            Dim predictedY As Single = (Slope * xValue) + Intercept
            Return predictedY


        End Function

        Public Shared Function CalculateTScore(ByVal Slope As Single, ByVal standardError As Single) As Single

            Dim tScore As Single = Slope / standardError

            Return tScore

        End Function

        Public Shared Function CalculateProbability(ByVal tScore As Single) As Single

            Dim probability As Single = Nothing

            Return probability

        End Function

        Public Shared Function LinearRegression(ByVal paramListX As List(Of Single), ByVal paramListY As List(Of Single)) As LinearRegressionResults

            Dim results As New LinearRegressionResults

            Dim ySum As Single = paramListY.Sum
            Dim xSum As Single = paramListX.Sum
            Dim xSumOfSquares As Single = GetSumOfSquares(paramListX)
            Dim xySum As Single = MultiplyTwoLists(paramListX, paramListY).Sum
            Dim n As Integer = paramListX.Count

            'Calculate Intercept
            Dim interceptNumerator As Single = ((ySum * xSumOfSquares) - (xSum * xySum))
            Dim interceptDenomenator As Single = ((n * xSumOfSquares) - (Math.Pow(xSum, 2)))
            Dim Intercept As Single = interceptNumerator / interceptDenomenator

            'Calculate Slope
            Dim slopeNumerator As Single = ((n * xySum) - (xSum * ySum))
            Dim slopeDenomenator As Single = ((n * xSumOfSquares) - (Math.Pow(xSum, 2)))
            Dim Slope As Single = slopeNumerator / slopeDenomenator
            Dim standardErrorOfSlope As Single = CalculateStandardErrorOfSlope(paramListX, paramListY, Slope, Intercept)
            Dim tScore As Single = CalculateTScore(Slope, standardErrorOfSlope)
            Dim probability As Single = CalculateProbability(tScore)

            'MsgBox("Standard Error: " & standardErrorOfSlope.ToString)
            results.Slope = Slope
            results.Intercept = Intercept
            results.DegreesOfFreedom = paramListX.Count - 2
            results.Tscore = tScore
            results.Probability = probability

            Return results

        End Function

        Public Shared Function SimpleLinearRegressionANOVA(ByVal xValueList As List(Of Single),
                                                      ByVal yValueList As List(Of Single),
                                                      ByVal Slope As Single,
                                                      ByVal Intercept As Single) As Single


            Dim yPredictedList As New List(Of Single)
            For i As Integer = 0 To yValueList.Count - 1

                yPredictedList.Add(CalculatePredictedY(Slope, Intercept, xValueList.Item(i)))

            Next

            Dim yPredictedMean As Single = yPredictedList.Sum / yPredictedList.Count

            Dim ssrList As New List(Of Single)
            For i As Integer = 0 To yPredictedList.Count - 1

                ssrList.Add(yPredictedList(i) - yPredictedMean)

            Next

            Dim SSR As Single = Math.Pow(Math.Sqrt(ssrList.Sum), 2.0)

            Dim sseList As New List(Of Single)
            For i As Integer = 0 To yValueList.Count - 1

                sseList.Add(yValueList.Item(i) - CalculatePredictedY(Slope, Intercept, xValueList.Item(i)))

            Next

            Dim SSE As Single = Math.Pow(Math.Sqrt(sseList.Sum), 2.0)

            Dim MSE As Single = SSE / yValueList.Count - 2
            Dim fScore As Single = SSR / MSE

            Return MSE

        End Function

        Public Shared Function CalculateANOVA_ForCrispGroups(ByVal paramList1 As List(Of Single),
                                                             ByVal medianParameter1 As Single,
                                                             ByVal paramList2 As List(Of Single),
                                                             ByVal medianParameter2 As Single)

            'Dim subsetParam1 As New List(Of Single)
            'For i As Integer = 0 To 3
            'Dim predicate = Function(list As List(Of Single))
            '                    list.Contains(medianParameter1)
            '                End Function
            'Dim subsetParam1 As List(Of Single) = paramList1.FindAll(AddressOf getParameterClassLow(medianParameter1))
            'Next

            Dim subsetParam1 As List(Of Single) = paramList1.FindAll(Function(p) p > medianParameter1 And p < medianParameter2)


            Return subsetParam1
        End Function

        Public Shared Function GetSubsetMean(ByVal uncertaintyAnalysisObject As UncertaintyAnalysis, ByVal parameterValues As UncertaintyAnalysis.Parameter, ByVal criteria As UShort)

            Dim tempList As New List(Of Single)
            For i As Integer = 0 To uncertaintyAnalysisObject.Uncertainty.Count - 1
                If parameterValues.ClassifiedValues.Item(i) = criteria Then
                    tempList.Add(uncertaintyAnalysisObject.Uncertainty.Item(i))
                End If
            Next

            Dim meanAndCount As TupleMeanAndCount
            meanAndCount.addValues(tempList.Average, tempList.Count)

            'Dim meanAndCount As New Tuple(Of Single, Integer)(tempList.Average, tempList.Count)

            Return meanAndCount

        End Function

        'Public Shared Function GetDegreesOfFreedomANOVA_Column(ByVal classifiedList As List(Of UShort), ByVal numberOfTreatmentGroups As Integer, ByVal subsetName1 As Integer, ByVal subsetName2 As Integer)

        '    Dim DegreesOfFreedomTreatment As Integer = numberOfTreatmentGroups - 1

        '    Dim DegreesOfFreedomTotal As Integer = Nothing 'valueList.Values.Count - 1
        '    If numberOfTreatmentGroups = 2 Then
        '        DegreesOfFreedomTotal = classifiedList.FindAll(Function(p) p = subsetName1 Or p = subsetName2).Count - 1
        '    ElseIf numberOfTreatmentGroups = 3 Then
        '        DegreesOfFreedomTotal = classifiedList.Count - 1
        '    End If

        '    Dim DegreesOfFreedomResidual As Integer = DegreesOfFreedomTotal - DegreesOfFreedomTreatment
        '    Dim DegreesOfFreedomColumn As New Tuple(Of Integer, Integer, Integer)(DegreesOfFreedomTreatment, DegreesOfFreedomResidual, DegreesOfFreedomTotal)

        '    Return DegreesOfFreedomColumn

        'End Function

        'Public Shared Function GetSumOfSquaresANOVA_Column(ByVal uncertaintyAnalysisObject As UncertaintyAnalysis, ByVal parameterValues As UncertaintyAnalysis.Parameter)

        '    'Get means of population of treatment groups
        '    Dim overallMean As Single = uncertaintyAnalysisObject.Uncertainty.Average
        '    'Dim group1Mean As Tuple(Of Single, Integer) = GetSubsetMean(uncertaintyAnalysisObject, parameterValues, 1)
        '    Dim group1Mean As TopCAT.Stats.TupleMeanAndCount = GetSubsetMean(uncertaintyAnalysisObject, parameterValues, 1)
        '    'Dim group2Mean As Tuple(Of Single, Integer) = GetSubsetMean(uncertaintyAnalysisObject, parameterValues, 2)
        '    Dim group2Mean As TopCAT.Stats.TupleMeanAndCount = GetSubsetMean(uncertaintyAnalysisObject, parameterValues, 2)
        '    'Dim group3Mean As Tuple(Of Single, Integer) = GetSubsetMean(uncertaintyAnalysisObject, parameterValues, 3)

        '    'Get the estimated effect for each treatment group
        '    'Dim estimatedEffectGroup1 As Single = group1Mean.Item1 - overallMean
        '    Dim estimatedEffectGroup1 As Single = group1Mean.Mean - overallMean
        '    'Dim estimatedEffectGroup2 As Single = group2Mean.Item1 - overallMean
        '    Dim estimatedEffectGroup2 As Single = group2Mean.Mean - overallMean
        '    ' Dim estimatedEffectGroup3 As Single = group3Mean.Item1 - overallMean

        '    'Get the sum of squares between treatment groups
        '    'Dim SumOfSquaresBetweenTreatmentGroups As Single = (Math.Pow(estimatedEffectGroup1, 2) * group1Mean.Item2) + (Math.Pow(estimatedEffectGroup2, 2) * group2Mean.Item2) ' + (Math.Pow(estimatedEffectGroup3, 2) * group3Mean.Item2)
        '    Dim SumOfSquaresBetweenTreatmentGroups As Single = (Math.Pow(estimatedEffectGroup1, 2) * group1Mean.Count) + (Math.Pow(estimatedEffectGroup2, 2) * group2Mean.Count) ' + (Math.Pow(estimatedEffectGroup3, 2) * group3Mean.Item2)


        '    'Calculate sum of sqaures within treatment groups 
        '    Dim tempList1 As New List(Of Single)
        '    Dim tempList2 As New List(Of Single)
        '    Dim tempList3 As New List(Of Single)
        '    For i As Integer = 0 To parameterValues.Values.Count - 1
        '        If parameterValues.ClassifiedValues.Item(i) = 1 Then
        '            'tempList1.Add(Math.Pow((uncertaintyAnalysisObject.Uncertainty.Item(i) - group1Mean.Item1), 2))
        '            tempList1.Add(Math.Pow((uncertaintyAnalysisObject.Uncertainty.Item(i) - group1Mean.Mean), 2))
        '        ElseIf parameterValues.ClassifiedValues.Item(i) = 2 Then
        '            'tempList2.Add(Math.Pow((uncertaintyAnalysisObject.Uncertainty.Item(i) - group2Mean.Item1), 2))
        '            tempList2.Add(Math.Pow((uncertaintyAnalysisObject.Uncertainty.Item(i) - group2Mean.Mean), 2))
        '            'ElseIf parameterValues.ClassifiedValues.Item(i) = 3 Then
        '            '    tempList3.Add(Math.Pow((uncertaintyAnalysisObject.Uncertainty.Item(i) - group3Mean.Item1), 2))
        '        End If
        '    Next

        '    Dim SumOfSquaresWithinTreatmentGroups As Single = tempList1.Sum + tempList2.Sum + tempList3.Sum
        '    Dim SumOfSquaresTotal As Single = SumOfSquaresBetweenTreatmentGroups + SumOfSquaresWithinTreatmentGroups
        '    Dim SumOfSquaresColumn As New Tuple(Of Single, Single, Single)(SumOfSquaresBetweenTreatmentGroups, SumOfSquaresWithinTreatmentGroups, SumOfSquaresTotal)

        '    Return SumOfSquaresColumn
        'End Function

        'Public Shared Function GetMeanSquaresANOVA_Column(ByVal ssColumn As Tuple(Of Single, Single, Single), ByVal dfColumn As Tuple(Of Integer, Integer, Integer))

        '    Dim MeanSquaresTreatment As Single = ssColumn.Item1 / dfColumn.Item1
        '    Dim MeanSquaresResidual As Single = ssColumn.Item2 / dfColumn.Item2
        '    Dim FValue As Single = MeanSquaresTreatment / MeanSquaresResidual

        '    Return FValue
        'End Function

        Public Shared Function ClassifyRasterValues(ByVal rasterValList As List(Of Single), ByVal median As Single, ByVal standardDeviation As Single) As List(Of UShort)

            Dim classifiedValues As New List(Of UShort)

            For i As Integer = 0 To rasterValList.Count - 1

                Select Case rasterValList.Item(i)

                    Case Is <= median
                        classifiedValues.Add(1)

                    Case median To median + (standardDeviation * 2)
                        classifiedValues.Add(2)

                    Case Is > median + (standardDeviation * 2)
                        classifiedValues.Add(3)

                End Select

            Next
            Return classifiedValues

        End Function

        Public Shared Function ClassifyRasterValues(ByVal rasterValList As List(Of Double), ByVal median As Single, ByVal standardDeviation As Single) As List(Of UShort)

            Dim classifiedValues As New List(Of UShort)

            For i As Integer = 0 To rasterValList.Count - 1

                Select Case rasterValList.Item(i)

                    Case Is <= median
                        classifiedValues.Add(1)

                    Case median To median + (standardDeviation * 2)
                        classifiedValues.Add(2)

                    Case Is > median + (standardDeviation * 2)
                        classifiedValues.Add(3)

                End Select

            Next
            Return classifiedValues

        End Function
    End Class
End Namespace
