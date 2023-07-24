﻿namespace Chair.BLL.Dto.ExecutorService
{
    public class AddExecutorServiceDto
    {
        public Guid ServiceTypeId { get; set; }
        public Guid ExecutorId { get; set; }
        public string Description { get; set; }
        public List<string> ImageURLs { get; set; }
    }
}
