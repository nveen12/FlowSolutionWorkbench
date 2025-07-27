Public Class safetyStockCalculation


    ''' <summary>
    ''' Calculates a fixed month supply according to the number of months you hand over.
    ''' </summary>
    ''' <param name="months">The number of months you want to stock.</param>
    ''' <param name="organizationId">The organization you want to point at.</param>
    ''' <param name="inventoryItemId">The inventory item id you are looking for.</param>
    ''' <returns>The </returns>
    ''' <remarks></remarks>
    Public Function calculateMonthlySupply(ByVal months As Double, ByVal organizationId As Integer, ByVal inventoryItemId As Integer)

        Dim dblQuantity As Double = 0
        'Logic to get the average monthly consumption
        Dim dblMonthlyConsumtion As Double = 3
        Return dblQuantity

    End Function

    'Implement forecast models ...

End Class
