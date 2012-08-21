Namespace Entities
    Public Class RecertificationCircleValidation
        Public ReadOnly Property StartDate As DateTime
            Get
                Dim result = ExpirationDate.AddMonths(0 - ValidilityMonths)
                Return result
            End Get
        End Property
        Public Property ExpirationDate As DateTime
        Public Property ValidilityMonths As Integer
    End Class
End Namespace