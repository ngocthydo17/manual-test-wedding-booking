using Google.Cloud.Firestore;

namespace doan.Models
{
    [FirestoreData]  // Thêm thuộc tính này để cho Firestore biết đây là một lớp dữ liệu
    public class diuyn
    {
        [FirestoreProperty]
        public string ten1 { get; set; }

        [FirestoreProperty]
        public double gia1 { get; set; }

        [FirestoreProperty]
        public string anh1 { get; set; }

        [FirestoreProperty]
        public string mota1 { get; set; }

       
        // Constructor mặc định
        public diuyn() { }

        // Constructor tùy chỉnh
        public diuyn(string ten1, double gia1, string anh1, string mot1)
        {
            ten1 = ten1;
            gia1 = gia1;
            anh1 = anh1;
            mota1 = mota1;
           
        }
    }
}
