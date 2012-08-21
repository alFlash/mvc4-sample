Imports System
Imports System.Collections
Imports TIMSS.API.DigitalContentDeliveryInfo
Imports TIMSS.API.MeetingInfo
Imports TIMSS.API.ProductInfo
Imports TIMSS.API.WebInfo

Namespace Personify.ApplicationManager.PersonifyDataObjects
    <Serializable> _
    Public Class ProductDetails
        Implements IDisposable
        ' Methods
        Public Sub New(ByVal oProductDetailHashTable As Hashtable)
            Me._htProductInfo = oProductDetailHashTable
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.Dispose(True)
            GC.SuppressFinalize(Me)
            Me._htProductInfo = Nothing
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        End Sub


        ' Properties
        Public ReadOnly Property AllPrices As WebPrices
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_AllPrices") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_AllPrices"), WebPrices)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property Components As ProductComponentList
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_Components") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_Components"), ProductComponentList)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property CrossSellProducts As ITmarWebProductViewList
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_CrossSell") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_CrossSell"), ITmarWebProductViewList)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property CustomerPrice As WebPrices
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_CustomerPrice") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_CustomerPrice"), WebPrices)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property DCDFileListing As IDigitalContentDeliverySetupList
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_ECDFileInfo") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_ECDFileInfo"), IDigitalContentDeliverySetupList)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property ListPrices As WebPrices
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_ListPrices") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_ListPrices"), WebPrices)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property MeetingInfo As IMeetingProducts
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_MeetingInfo") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_MeetingInfo"), IMeetingProducts)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property MemberPrices As WebPrices
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_MemberPrices") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_MemberPrices"), WebPrices)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property ProductId As Integer
            Get
                Return CInt(Me.ProductInfo.Item(0).ProductId)
            End Get
        End Property

        Public ReadOnly Property ProductInfo As ITmarWebProductViewList
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_WebProductInfo") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_WebProductInfo"), ITmarWebProductViewList)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property ProductType As String
            Get
                Return Me.ProductInfo.Item(0).ProductTypeCode.Code
            End Get
        End Property

        Public ReadOnly Property RelatedCustomers As IProductRelatedCustomers
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_RelatedCustomers") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_RelatedCustomers"), IProductRelatedCustomers)
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property SubProducts As ProductDetails()
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_SubProducts") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_SubProducts"), ProductDetails())
                End If
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property Subsystem As String
            Get
                Return Me.ProductInfo.Item(0).Subsystem
            End Get
        End Property

        Public ReadOnly Property UpSellProducts As ITmarWebProductViewList
            Get
                If (Not Me._htProductInfo.Item("ProductDetails_UpSell") Is Nothing) Then
                    Return DirectCast(Me._htProductInfo.Item("ProductDetails_UpSell"), ITmarWebProductViewList)
                End If
                Return Nothing
            End Get
        End Property


        ' Fields
        Private _htProductInfo As Hashtable
    End Class
End Namespace

