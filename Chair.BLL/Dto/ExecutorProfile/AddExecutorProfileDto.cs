﻿using Chair.BLL.Dto.Contacts;

namespace Chair.BLL.Dto.ExecutorService
{
    public class AddExecutorProfileDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
        public List<AddContactsDto> Contacts { get; set; }
    }
}