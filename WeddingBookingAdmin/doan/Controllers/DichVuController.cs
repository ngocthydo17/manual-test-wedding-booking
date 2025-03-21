using doan.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class DichVuController : Controller
    {
        // GET: DichVuController
        private readonly FirestoreDb _firestoreDb;



        public DichVuController(FirestoreDb firestoreDb)
        {
            _firestoreDb = firestoreDb;

        }
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                // Nếu không có từ khóa tìm kiếm được cung cấp, có thể chuyển hướng hoặc hiển thị một thông báo lỗi.
                return RedirectToAction(nameof(Index));
            }

            Query query = _firestoreDb.Collection("diuyn")
                                     .WhereEqualTo("ten1", searchTerm); // Ví dụ: Tìm kiếm theo tên món ăn

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            List<(string Id, diuyn Dish)> searchResults = new List<(string Id, diuyn Dish)>();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    diuyn dish = documentSnapshot.ConvertTo<diuyn>();
                    searchResults.Add((documentSnapshot.Id, dish));
                }
            }

            return View("Index", searchResults); // Chuyển hướng hoặc trả về view Index với kết quả tìm kiếm
        }

        public async Task<IActionResult> Index(string id)
        {
            Query allDishesQuery = _firestoreDb.Collection("diuyn");
            QuerySnapshot allDishesQuerySnapshot = await allDishesQuery.GetSnapshotAsync();
            List<(string Id, diuyn Dish)> dishes = new List<(string, diuyn)>();

            foreach (DocumentSnapshot documentSnapshot in allDishesQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    diuyn dish = documentSnapshot.ConvertTo<diuyn>();
                    string dishId = documentSnapshot.Id; // Assign document ID directly
                    dishes.Add((dishId, dish));
                }
            }

            // Sắp xếp danh sách món ăn theo tên
            var sortedDishes = dishes.OrderBy(d => d.Dish.ten1).ToList();

            return View(sortedDishes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] diuyn dish)
        {


            DocumentReference docRef = await _firestoreDb.Collection("diuyn").AddAsync(dish);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            DocumentSnapshot documentSnapshot = await _firestoreDb.Collection("diuyn").Document(id).GetSnapshotAsync();
            if (documentSnapshot.Exists)
            {
                diuyn dish = documentSnapshot.ConvertTo<diuyn>();
                return View(dish);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromForm] diuyn dish)
        {


            DocumentReference docRef = _firestoreDb.Collection("diuyn").Document(id);
            await docRef.SetAsync(dish, SetOptions.Overwrite);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            DocumentSnapshot documentSnapshot = await _firestoreDb.Collection("diuyn").Document(id).GetSnapshotAsync();
            if (documentSnapshot.Exists)
            {
                diuyn dish = documentSnapshot.ConvertTo<diuyn>();
                var model = new List<(string Id, diuyn Dish)>
        {
            (id, dish)
        };

                return View(model);
            }

            return NotFound();
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return BadRequest("Id cannot be null.");
            }

            DocumentReference docRef = _firestoreDb.Collection("diuyn").Document(id);
            await docRef.DeleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
