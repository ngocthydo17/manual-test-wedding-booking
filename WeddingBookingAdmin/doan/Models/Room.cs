using Google.Cloud.Firestore;

namespace doan.Models
{
    [FirestoreData]  // Thêm thuộc tính này để cho Firestore biết đây là một lớp dữ liệu
    public class Room
    {
        [FirestoreProperty]
        public string ten { get; set; }

        [FirestoreProperty]
        public double gia { get; set; }

        [FirestoreProperty]
        public string anh { get; set; }

        [FirestoreProperty]
        public string mota { get; set; }

        [FirestoreProperty]
        public double sl { get; set; }

        // Constructor mặc định
        public Room() { }

        // Constructor tùy chỉnh
        public Room(string ten, double gia, string anh, string mota, double sl)
        {
            ten = ten;
            gia = gia;
            anh = anh;
            mota = mota;
            sl = sl;
        }
    }
}
