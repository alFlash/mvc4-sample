Imports System
Imports System.Security.Cryptography

Namespace Personify.ApplicationManager
    Public Class RandomPassword
        ' Methods
        Public Shared Function Generate() As String
            Return RandomPassword.Generate(RandomPassword.DEFAULT_MIN_PASSWORD_LENGTH, RandomPassword.DEFAULT_MAX_PASSWORD_LENGTH)
        End Function

        Public Shared Function Generate(ByVal length As Integer) As String
            Return RandomPassword.Generate(length, length)
        End Function

        Public Shared Function Generate(ByVal minLength As Integer, ByVal maxLength As Integer) As String
            Dim num As Integer
            If (((minLength <= 0) Or (maxLength <= 0)) Or (minLength > maxLength)) Then
            End If
            Dim chArray As Char()() = New Char()() { RandomPassword.PASSWORD_CHARS_LCASE.ToCharArray, RandomPassword.PASSWORD_CHARS_UCASE.ToCharArray, RandomPassword.PASSWORD_CHARS_NUMERIC.ToCharArray }
            Dim numArray As Integer() = New Integer(((chArray.Length - 1) + 1)  - 1) {}
            Dim num9 As Integer = (numArray.Length - 1)
            num = 0
            Do While (num <= num9)
                numArray(num) = chArray(num).Length
                num += 1
            Loop
            Dim numArray2 As Integer() = New Integer(((chArray.Length - 1) + 1)  - 1) {}
            Dim num10 As Integer = (numArray2.Length - 1)
            num = 0
            Do While (num <= num10)
                numArray2(num) = num
                num += 1
            Loop
            Dim data As Byte() = New Byte(4 - 1) {}

            Dim p As System.Security.Cryptography.RNGCryptoServiceProvider = New System.Security.Cryptography.RNGCryptoServiceProvider()

            p.GetBytes(data)

            Dim seed As Integer = (((((data(0) And &H7F) << &H18) Or CByte((data(1) << 0))) Or CByte((data(2) << 0))) Or data(3))
            Dim random As New Random(seed)
            Dim chArray2 As Char() = Nothing
            If (minLength < maxLength) Then
                chArray2 = New Char((random.Next((minLength - 1), maxLength) + 1)  - 1) {}
            Else
                chArray2 = New Char(((minLength - 1) + 1)  - 1) {}
            End If
            Dim maxValue As Integer = (numArray2.Length - 1)
            Dim num11 As Integer = (chArray2.Length - 1)
            num = 0
            Do While (num <= num11)
                Dim num4 As Integer
                Dim num6 As Integer
                If (maxValue = 0) Then
                    num6 = 0
                Else
                    num6 = random.Next(0, maxValue)
                End If
                Dim index As Integer = numArray2(num6)
                Dim num2 As Integer = (numArray(index) - 1)
                If (num2 = 0) Then
                    num4 = 0
                Else
                    num4 = random.Next(0, (num2 + 1))
                End If
                chArray2(num) = chArray(index)(num4)
                If (num2 = 0) Then
                    numArray(index) = chArray(index).Length
                Else
                    If (num2 <> num4) Then
                        Dim ch As Char = chArray(index)(num2)
                        chArray(index)(num2) = chArray(index)(num4)
                        chArray(index)(num4) = ch
                    End If
                    numArray(index) -= 1
                End If
                If (maxValue = 0) Then
                    maxValue = (numArray2.Length - 1)
                Else
                    If (maxValue <> num6) Then
                        Dim num8 As Integer = numArray2(maxValue)
                        numArray2(maxValue) = numArray2(num6)
                        numArray2(num6) = num8
                    End If
                    maxValue -= 1
                End If
                num += 1
            Loop
            Return New String(chArray2)
        End Function


        ' Fields
        Private Shared DEFAULT_MAX_PASSWORD_LENGTH As Integer = 10
        Private Shared DEFAULT_MIN_PASSWORD_LENGTH As Integer = 8
        Private Shared PASSWORD_CHARS_LCASE As String = "abcdefgijkmnopqrstwxyz"
        Private Shared PASSWORD_CHARS_NUMERIC As String = "23456789"
        Private Shared PASSWORD_CHARS_UCASE As String = "ABCDEFGHJKLMNPQRSTWXYZ"
    End Class
End Namespace

