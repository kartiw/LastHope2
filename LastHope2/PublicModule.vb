Module PublicModule
    'AddNewSubLeader
    Public mainleaderid As String

    'ChnageLeader
    Public oldmainleaderid As String
    Public subleaderid As String

    'AddNewRelation(Partner Details)
    Public ARParntnerID As String

    'EditRelation (Partner Details)
    Public ERPartnerID As String
    Public relationtable As New DataTable

    'SearchPartner
    Public SPPartnerID As String
    Public SPPartnerName As String

    'SearchLeader
    Public SLPartnerID As String
    Public SLPartnerName As String

    'ViewCategory
    Public VCCategoryID As String
    Public VCCategoryName As String

    'SearchPartner - Area
    Public SPAreaID As String


End Module
