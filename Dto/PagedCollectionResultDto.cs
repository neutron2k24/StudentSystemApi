﻿namespace StudentSystem.Dto {
    /// <summary>
    /// Defines a collection of DTOs
    /// </summary>
    public class PagedCollectionResultDto<T> {
        
        public List<T>? Results { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? PageCount { get; set; }

        public int? ResultCount { get; set; }
        public int? TotalCount { get; set; }
    }
}
