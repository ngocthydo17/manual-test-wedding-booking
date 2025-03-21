using doan.Models;
using Google.Cloud.Firestore;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;

public class AllDishController : Controller
{
    private readonly FirestoreDb _firestoreDb;
   


    public AllDishController(FirestoreDb firestoreDb)
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

        Query query = _firestoreDb.Collection("AllDish")
                                 .WhereEqualTo("name", searchTerm); // Ví dụ: Tìm kiếm theo tên món ăn

        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

        List<(string Id, AllDish Dish)> searchResults = new List<(string Id, AllDish Dish)>();

        foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
        {
            if (documentSnapshot.Exists)
            {
                AllDish dish = documentSnapshot.ConvertTo<AllDish>();
                searchResults.Add((documentSnapshot.Id, dish));
            }
        }

        return View("Index", searchResults); // Chuyển hướng hoặc trả về view Index với kết quả tìm kiếm
    }

    public async Task<IActionResult> Index(string id)
    {
        Query allDishesQuery = _firestoreDb.Collection("AllDish");
        QuerySnapshot allDishesQuerySnapshot = await allDishesQuery.GetSnapshotAsync();
        List<(string Id, AllDish Dish)> dishes = new List<(string, AllDish)>();

        foreach (DocumentSnapshot documentSnapshot in allDishesQuerySnapshot.Documents)
        {
            if (documentSnapshot.Exists)
            {
                AllDish dish = documentSnapshot.ConvertTo<AllDish>();

                string dishId = documentSnapshot.Id; // Assign document ID directly
                dishes.Add((dishId, dish));
            }
        }

        // Sắp xếp danh sách món ăn theo tên
        var sortedDishes = dishes.OrderBy(d => d.Dish.name).ToList();

        return View(sortedDishes);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] AllDish dish)
    {
      

        DocumentReference docRef = await _firestoreDb.Collection("AllDish").AddAsync(dish);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id)
    {
        DocumentSnapshot documentSnapshot = await _firestoreDb.Collection("AllDish").Document(id).GetSnapshotAsync();
        if (documentSnapshot.Exists)
        {
            AllDish dish = documentSnapshot.ConvertTo<AllDish>();
            return View(dish);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string id, [FromForm] AllDish dish)
    {
       

        DocumentReference docRef = _firestoreDb.Collection("AllDish").Document(id);
        await docRef.SetAsync(dish, SetOptions.Overwrite);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string id)
    {
        DocumentSnapshot documentSnapshot = await _firestoreDb.Collection("AllDish").Document(id).GetSnapshotAsync();
        if (documentSnapshot.Exists)
        {
            AllDish dish = documentSnapshot.ConvertTo<AllDish>();
            var model = new List<(string Id, AllDish Dish)>
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

        DocumentReference docRef = _firestoreDb.Collection("AllDish").Document(id);
        await docRef.DeleteAsync();
        return RedirectToAction(nameof(Index));
    }

 
}
