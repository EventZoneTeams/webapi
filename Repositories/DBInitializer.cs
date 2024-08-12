using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Repositories.Utils;

namespace Repositories
{
    public static class DBInitializer
    {
        public static async Task Initialize(StudentEventForumDbContext context, UserManager<User> userManager)
        {
            Random random = new Random();

            if (!context.EventCategories.Any())
            {
                var eventCategories = new List<EventCategory>()
            {
                              new EventCategory
                              {
                                Title = "Education",
                                ImageUrl = "https://www.timeshighereducation.com/student/sites/default/files/istock-499343530.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Music",
                                ImageUrl = "https://artsreview.b-cdn.net/wp-content/uploads/2022/06/A-crowd-at-a-music-concert.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Sports",
                                ImageUrl = "https://www.coe.int/documents/24916852/0/Supporters3.jpg/63b405d6-be6d-d2ec-bd11-0f03c6ca8130?t=1503560660000"
                              },
                              new EventCategory
                              {
                                Title = "Technology",
                                ImageUrl = "https://blog.bishopmccann.com/hubfs/event%20tech%20main_%20AdobeStock_352613171.jpeg#keepProtocol"
                              },
                              new EventCategory
                              {
                                Title = "Business",
                                ImageUrl = "https://www.loghicconnect.com.au/wp-content/uploads/2020/05/Untitled-design-2023-03-16T112342.403.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Art",
                                ImageUrl = "https://freeyorkk.b-cdn.net/wp-content/uploads/2021/06/AdobeStock_380232446-2400x1233.jpeg"
                              },
                              new EventCategory
                              {
                                Title = "Food & Drink",
                                ImageUrl = "https://goeshow.com/wp-content/uploads/2022/11/Blog-Image-10112022-700x423.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Travel",
                                ImageUrl = "https://au.travelctm.com/wp-content/uploads/2019/10/Double-Image-Incentive-600x438.jpg"
                              },
                              new  EventCategory
                              {
                                Title = "Film",
                                ImageUrl = "https://images.lifestyleasia.com/wp-content/uploads/sites/7/2023/01/23162628/shutterstock_95844517.jpg"
                              },
                              new EventCategory
                              {
                                Title = "Other",
                                ImageUrl = "https://static.ra.co/images/promoter/za-theotherevents.jpg?dateUpdated=1576847763880s"
                              },
                            };

                foreach (var eventCategory in eventCategories)
                {
                    await context.EventCategories.AddAsync(eventCategory);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                            {
                                new Role { Name = "Admin", NormalizedName = "ADMIN" },
                                new Role { Name = "Manager", NormalizedName = "MANAGER" },
                                new Role { Name = "Student", NormalizedName = "STUDENT" }
                            };

                foreach (var role in roles)
                {
                    await context.Roles.AddAsync(role);
                }

                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FullName = "admin",
                    Dob = DateTime.Now,
                    PhoneNumber = "0123456789",
                    Gender = false,
                    University = "FPT"
                };
                //Add roles
                await userManager.CreateAsync(admin, "123456");
                await userManager.AddToRoleAsync(admin, "Admin");

                var manager = new User
                {
                    UserName = "manager",
                    Email = "manager@gmail.com",
                    FullName = "Quan ly",
                    Dob = DateTime.Now,
                    PhoneNumber = "0123456789",
                    Gender = false,
                    University = "FPT"
                };
                //Add roles
                await userManager.CreateAsync(manager, "123456");
                await userManager.AddToRoleAsync(manager, "Manager");

                var student = new User
                {
                    UserName = "student",
                    Email = "student@gmail.com",
                    FullName = "Hoang Tien",
                    Dob = DateTime.Now,
                    PhoneNumber = "0123456789",
                    Gender = false,
                    University = "FPT"
                };
                //Add roles
                await userManager.CreateAsync(student, "123456");
                await userManager.AddToRoleAsync(student, "Student");

                var uydev = new User
                {
                    UserName = "uydev",
                    Email = "lequocuy@gmail.com",
                    FullName = "Lê Quốc Uy",
                    UnsignFullName = "Le Quoc Uy",
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 7, 11),
                    Gender = true,
                    Image = "https://scontent.fsgn15-1.fna.fbcdn.net/v/t39.30808-1/430878538_2206677789683723_4464660377243750146_n.jpg?stp=dst-jpg_p200x200&_nc_cat=106&ccb=1-7&_nc_sid=5f2048&_nc_eui2=AeE_Vr1x6BHZ_S__ovdDg7zS5W9udhABzaHlb252EAHNoS38q_urtNeTErRYpa0zqYNo-vOAf49-zjjLBslYOw-p&_nc_ohc=8En2AdNVtaUQ7kNvgEn1g25&_nc_ht=scontent.fsgn15-1.fna&oh=00_AYA_Dyr3Kzs4J5lFKCiaYlu6-KlRK4icdur4m-IrU68PPA&oe=664E1D9B"
                };
                //Add roles
                await userManager.CreateAsync(uydev, "123456");
                await userManager.AddToRoleAsync(uydev, "Student");

                var namthhse172294 = new User
                {
                    UserName = "namthhse172294",
                    Email = "namthhse172294@fpt.edu.vn",
                    FullName = "Trương Hà Hào Nam",
                    UnsignFullName = StringTools.ConvertToUnSign("Trương Hà Hào Nam"),
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 1, 1), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=namthhse172294" // Replace with the actual image URL
                };

                await userManager.CreateAsync(namthhse172294, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(namthhse172294, "Student");

                var vunse172437 = new User
                {
                    UserName = "vunse172437",
                    Email = "vunse172437@fpt.edu.vn",
                    FullName = "Nguyễn Vũ",
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 2, 15), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=vunse172437" // Replace with the actual image URL
                };

                await userManager.CreateAsync(vunse172437, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(vunse172437, "Student");

                var huanngse171018 = new User
                {
                    UserName = "huanngse171018",
                    Email = "huanngse171018@fpt.edu.vn",
                    FullName = "Ngô Gia Huấn",
                    UnsignFullName = StringTools.ConvertToUnSign("Ngô Gia Huấn"),
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 3, 20), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=huanngse171018" // Replace with the actual image URL
                };

                await userManager.CreateAsync(huanngse171018, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(huanngse171018, "Student");

                var tienhmse172436 = new User
                {
                    UserName = "tienhmse172436",
                    Email = "tienhmse172436@fpt.edu.vn",
                    FullName = "Hoàng Minh Tiến Lmao",
                    UnsignFullName = StringTools.ConvertToUnSign("Hoàng Minh Tiến"),
                    University = "FPTU HCM",
                    Dob = new DateTime(2003, 4, 5), // Replace with the actual date of birth
                    Gender = true, // Assuming true means male
                    Image = "https://avatar.iran.liara.run/public/boy?username=tienhmse172436" // Replace with the actual image URL
                };

                await userManager.CreateAsync(tienhmse172436, "123456"); // Replace "password" with the actual password
                await userManager.AddToRoleAsync(tienhmse172436, "Student");

                await context.SaveChangesAsync();
            }

            //            if (!context.Events.Any())
            //            {
            //                var events = new List<Event>
            //                {
            //                    new Event
            //                    {
            //                      Name = "Hội thảo Công nghệ 4.0",
            //            Description = "Khám phá những xu hướng mới nhất trong lĩnh vực công nghệ.",
            //            ThumbnailUrl = "https://picsum.photos/500/300/?image=10", // Example image URL
            //            EventStartDate = DateTime.Now.AddDays(15),
            //            EventEndDate = DateTime.Now.AddDays(16),
            //            Location = "FPT Cần Thơ",
            //            UserId = 1,
            //            EventCategoryId = 1,
            //            University = "FPTU Cần Thơ",
            //            Status = "Upcoming", // Ví dụ trạng thái: Upcoming, Ongoing, Finished
            //            CreatedAt = DateTime.Now,
            //                    },
            //                    new Event
            //                    {
            //              Name = "Đêm nhạc Acoustic",
            //            Description = "Thưởng thức những giai điệu nhẹ nhàng và sâu lắng.",
            //            ThumbnailUrl = "https://picsum.photos/500/300/?image=20",
            //            EventStartDate = DateTime.Now.AddDays(30),
            //            EventEndDate = DateTime.Now.AddDays(30),
            //            Location = "FPT HCM",
            //            UserId = 1,
            //            EventCategoryId = 1,
            //            University = "FPTU HCM",
            //            Status = "Upcoming",
            //            CreatedAt = DateTime.Now,
            //                    },
            //                     new Event
            //        {
            //            Name = "Workshop Khởi nghiệp",
            //            Description = "Hướng dẫn các bước để xây dựng một startup thành công.",
            //            ThumbnailUrl = "https://picsum.photos/500/300/?image=30",
            //            EventStartDate = DateTime.Now.AddDays(20),
            //            EventEndDate = DateTime.Now.AddDays(20),
            //            Location = "FPT Đà Nẵng",
            //            UserId = 1,
            //            EventCategoryId = 1,
            //            University = "FPTU Đà Nẵng",
            //            Status = "Upcoming",
            //            CreatedAt = DateTime.Now,
            //        }
            //                };

            //                await context.Events.AddRangeAsync(events);
            //                await context.SaveChangesAsync();
            //            }
            //            var existingEvents = await context.Events.ToListAsync(); // get để tham chiếu

            //            if (!context.EventProducts.Any())
            //            {
            //                var eventProducts = new List<EventProduct>
            //                {
            //                    new EventProduct
            //                    {
            //                        Name = "Product 1",
            //                        Description = "Description 1",
            //                        Price = 1000,
            //                        EventId = existingEvents.ElementAt(random.Next(1, existingEvents.Count)).Id,
            //                        QuantityInStock= random.Next(10, 1000),
            //                        CreatedAt=DateTime.Now,
            //                    },
            //                    new EventProduct
            //                    {
            //                        Name = "Product 2",
            //                        Description = "Description 2",
            //                        Price = 1000,
            //                        EventId = existingEvents.ElementAt(random.Next(1, existingEvents.Count)).Id,
            //                        QuantityInStock= random.Next(10, 500),
            //                       CreatedAt=DateTime.Now,
            //                    },
            //                    new EventProduct
            //                    {
            //                        Name = "Product 3",
            //                        Description = "Description 2",
            //                        Price = 5000,
            //                        EventId = existingEvents.ElementAt(random.Next(1, existingEvents.Count)).Id,
            //                        QuantityInStock= random.Next(10, 100),
            //                       CreatedAt=DateTime.Now,
            //                    }
            //                };

            //                await context.EventProducts.AddRangeAsync(eventProducts);

            //                await context.SaveChangesAsync();
            //            }

            //            // Seed data cho ProductImage
            //            if (!context.ProductImages.Any())
            //            {
            //                var eventProducts = await context.EventProducts.ToListAsync();

            //                var productImages = new List<ProductImage>();
            //                foreach (var eventProduct in eventProducts)
            //                {
            //                    productImages.Add(new ProductImage
            //                    {
            //                        ProductId = eventProduct.Id,
            //                        ImageUrl = $"https://picsum.photos/500/300/?image={random.Next(100, 200)}", // Example image URL
            //                        Name = $"Hình ảnh  của sản phẩm {eventProduct.Name}",
            //                        CreatedAt = DateTime.Now,
            //                        EventProduct = eventProduct
            //                    });
            //                }

            //                await context.ProductImages.AddRangeAsync(productImages);
            //                try
            //                {
            //                    var result = await context.SaveChangesAsync();
            //                }
            //                catch (Exception)
            //                {
            //                    throw;
            //                }
            //            }

            //            // seed cho packages
            //            if (!context.EventPackages.Any())
            //            {
            //                var eventPackages = new List<EventPackage>();

            //                // Tạo EventPackage trước
            //                foreach (var evt in existingEvents)
            //                {
            //                    for (int i = 0; i < 2; i++) // Tạo 3 gói package cho mỗi event
            //                    {
            //                        var eventPackage = new EventPackage
            //                        {
            //                            EventId = evt.Id,
            //                            Title = $"Gói {i + 1} cho sự kiện {evt.Name}", // Ví dụ: Gói 1, Gói 2, Gói 3
            //                            ThumbnailUrl = $"https://picsum.photos/500/300/?image={random.Next(200, 300)}",
            //                            Description = $"Mô tả gói {i + 1} cho sự kiện {evt.Name}",
            //                            CreatedAt = DateTime.Now,
            //                        };

            //                        eventPackages.Add(eventPackage);
            //                    }
            //                }

            //                await context.EventPackages.AddRangeAsync(eventPackages);
            //                await context.SaveChangesAsync();
            //            }

            //            //seed data product package
            //            if (!context.ProductInPackages.Any())
            //            {
            //                var eventPackages = await context.EventPackages.ToListAsync();
            //                var products = await context.EventProducts.ToListAsync();

            //                var productInPackages = new List<ProductInPackage>();
            //                foreach (var package in eventPackages)
            //                {
            //                    // Lấy một số ngẫu nhiên sản phẩm từ sự kiện
            //                    var selectedProducts = products.Where(p => p.EventId == package.EventId)
            //                                                    .OrderBy(p => Guid.NewGuid())
            //                                                    .Take(random.Next(1, 4))
            //                                                    .ToList();

            //                    foreach (var product in selectedProducts)
            //                    {
            //                        var productInPackage = new ProductInPackage
            //                        {
            //                            ProductId = product.Id,
            //                            PackageId = package.Id,
            //                            Quantity = random.Next(1, 5),
            //                        };
            //                        productInPackages.Add(productInPackage);
            //                    }

            //                    // Tính tổng giá của gói sản phẩm
            //                    package.TotalPrice = selectedProducts.Sum(p => p.Price * productInPackages.FirstOrDefault(pip => pip.ProductId == p.Id)?.Quantity ?? 0);
            //                }

            //                await context.ProductInPackages.AddRangeAsync(productInPackages);
            //                try
            //                {
            //                    await context.SaveChangesAsync();
            //                }
            //                catch (Exception ex)
            //                {
            //                    // Xử lý lỗi ở đây (ví dụ: ghi log, thông báo lỗi)
            //                    throw;
            //                }
            //            }
        }
    }
}