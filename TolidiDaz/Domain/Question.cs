namespace Domain
{
    /// <summary>
    ///  سوالات متداول
    /// </summary>
    public class Question : Base
    {
        public int? GroupQuestionID { get; set; }
        public string?  Title { get; set; }
        public string?  SubTitle { get; set; }
        public string? Description { get; set; }
        #region RelationShip
        public virtual GroupQuestion? GroupQuestion { get; set; }
        #endregion


    }
}
