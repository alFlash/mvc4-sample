Namespace Entities
    Public Class FieldInfo
        Public Property FormId() As String
        Public Property SectionId() As String
        Public Property FieldId() As String
        Public Property FieldName() As String
        'Text display in Administrator Control Panel
        Public Property FieldValue() As String
        'Text display in each form
        Public Property IsEnabled() As Boolean
        Public Property IsRequired() As Boolean
        Public Property IsReadOnly() As Boolean
        Public Property FieldTextType() As String
        Public Property ValidateControls() As List(Of ValidateControlInfo)
        Public Property QuestionList() As List(Of QuestionInfo)
        Public Property Sequence() As Integer
        Public Property IsRichText() As Boolean
        Public Property IsQuestion() As Boolean
        Public Property SurveyTitle() As String
        Public Property IsInstruction() As Boolean
        Public Property IsConfigurable() As Boolean
        Public Property IsCalculate() As Boolean
        Public Property CalculateFormula() As Decimal
        Public Property QuestionType() As String
        Public Property CanAddQuestion() As Boolean
        Public Const KEY = "FieldId"
    End Class
End Namespace