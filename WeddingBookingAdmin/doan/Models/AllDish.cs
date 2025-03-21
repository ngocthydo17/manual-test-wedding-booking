using Google.Cloud.Firestore;

namespace doan.Models
{
    [FirestoreData]
    public class AllDish
    {
     

        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public double price { get; set; }

        [FirestoreProperty]
        public String image { get; set; }

        [FirestoreProperty]
        public String mota { get; set; }

        [FirestoreProperty]
        public String cate { get; set; }


        // Constructor mặc định
        public AllDish() { }

        // Constructor tùy chỉnh
        public AllDish( string name, decimal price,String mota, String image,String cate)
        {
            
            name = name;
            price = price;
            mota = mota;
            image = image;
            cate = cate;

        }
    }
}
