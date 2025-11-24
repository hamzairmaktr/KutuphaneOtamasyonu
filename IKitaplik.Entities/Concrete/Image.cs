using Core.Entities;
using IKitaplik.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.Concrete
{
    public class Image : BaseEntities
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public ImageType ImageType { get; set; }
        public int RelationshipId { get; set; }
        public User User { get; set; }

    }
}
