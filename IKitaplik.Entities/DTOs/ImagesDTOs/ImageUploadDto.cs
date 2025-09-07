using IKitaplik.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.ImagesDTOs
{
    public class ImageUploadDto
    {
        public ImageType ImageType { get; set; }
        public int RelationshipId { get; set; }
        public IFormFile File { get; set; }
    }
}
