namespace StudentSystem.Dto {
    /// <summary>
    /// Define a link dto for HATEOAS
    /// </summary>
    public class LinkDto {
        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Type {  get; set; }
    }
}
