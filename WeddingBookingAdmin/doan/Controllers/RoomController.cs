using doan.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace doan.Controllers
{
    public class RoomController : Controller
    {
        // GET: RoomController
        private readonly FirestoreDb _firestoreDb;



        public RoomController(FirestoreDb firestoreDb)
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

            Query query = _firestoreDb.Collection("Room")
                                     .WhereEqualTo("ten", searchTerm); // Ví dụ: Tìm kiếm theo tên món ăn

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            List<(string Id, Room Dish)> searchResults = new List<(string Id, Room Dish)>();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Room dish = documentSnapshot.ConvertTo<Room>();
                    searchResults.Add((documentSnapshot.Id, dish));
                }
            }

            return View("Index", searchResults); // Chuyển hướng hoặc trả về view Index với kết quả tìm kiếm
        }

        public async Task<IActionResult> Index(string id)
        {
            Query allDishesQuery = _firestoreDb.Collection("Room");
            QuerySnapshot allDishesQuerySnapshot = await allDishesQuery.GetSnapshotAsync();
            List<(string Id, Room Dish)> dishes = new List<(string, Room)>();

            foreach (DocumentSnapshot documentSnapshot in allDishesQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Room dish = documentSnapshot.ConvertTo<Room>();
                    string dishId = documentSnapshot.Id; // Assign document ID directly
                    dishes.Add((dishId, dish));
                }
            }

            // Sắp xếp danh sách món ăn theo tên
            var sortedDishes = dishes.OrderBy(d => d.Dish.ten).ToList();

            return View(sortedDishes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Room dish)
        {


            DocumentReference docRef = await _firestoreDb.Collection("Room").AddAsync(dish);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            DocumentSnapshot documentSnapshot = await _firestoreDb.Collection("Room").Document(id).GetSnapshotAsync();
            if (documentSnapshot.Exists)
            {
                Room dish = documentSnapshot.ConvertTo<Room>();
                return View(dish);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, [FromForm] Room dish)
        {


            DocumentReference docRef = _firestoreDb.Collection("Room").Document(id);
            await docRef.SetAsync(dish, SetOptions.Overwrite);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            DocumentSnapshot documentSnapshot = await _firestoreDb.Collection("Room").Document(id).GetSnapshotAsync();
            if (documentSnapshot.Exists)
            {
                Room dish = documentSnapshot.ConvertTo<Room>();
                var model = new List<(string Id, Room Dish)>
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

            DocumentReference docRef = _firestoreDb.Collection("Room").Document(id);
            await docRef.DeleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
    }
