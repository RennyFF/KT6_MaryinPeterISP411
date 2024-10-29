using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopApp.Utils
{
    public static class ImageToBytes
    {
        public static void GetBytesFromImage()
        {
            foreach (var Product in Model.TradeEntities.GetContext().Product)
            {
                string path = Directory.GetCurrentDirectory() + "/Images/" + Product.ProductPhotoName;
                if (File.Exists(path))
                {
                    Product.ProductPhoto = File.ReadAllBytes(path);
                }
            }
            Model.TradeEntities.GetContext().SaveChanges();
        }
    }
}
