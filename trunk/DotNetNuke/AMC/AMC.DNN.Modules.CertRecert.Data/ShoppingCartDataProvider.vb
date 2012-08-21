Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports TIMSS.API.User.ProductInfo
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.ProductInfo


Partial Class PersonifyDataProvider
    Inherits DataProvider

    Private Function GetByProductId (productId As String) As IProduct

        Dim products As IProducts = CType ([Global].GetCollection (_organizationId,
                                                                   _organizationUnitId,
                                                                   NamespaceEnum.ProductInfo,
                                                                   "Products"
                                                                   ),
                                           IProducts)

        With products.Filter
            .Add ("ProductId", QueryOperatorEnum.Equals, productId)
        End With
        products.Fill()

        If products.Count > 0 Then
            Return products (0)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetProductReateCodeReateStructure (productId As String) As ProductRateCodeRateStructure
        Dim productRateCodeRateStructure As ProductRateCodeRateStructure = Nothing
        Try
            Dim product As IProduct = GetByProductId (productId)
            If Not product Is Nothing Then
                For Each pCode As IProductRateCode In product.RateCodes
                    'iterating trought the product codes
                    Dim prCode As ProductRateCode = TryCast (pCode, ProductRateCode)
                    If prCode IsNot Nothing Then
                        productRateCodeRateStructure = New ProductRateCodeRateStructure()
                        productRateCodeRateStructure.ProductRateCode = prCode.RateCode.Code
                        productRateCodeRateStructure.ProductRateStructure = prCode.RateStructure.Code
                    End If
                Next
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return productRateCodeRateStructure
    End Function

    Public Class ProductRateCodeRateStructure
        Public Property ProductRateCode() As String
        Public Property ProductRateStructure() As String
    End Class
End Class
