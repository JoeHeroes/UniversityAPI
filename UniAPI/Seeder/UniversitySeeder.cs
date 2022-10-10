using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UniAPI.Authorization;
using UniAPI.Entites;

namespace UniAPI.Seeder
{
    public class UniversitySeeder
    {
        private readonly UniversityDbContext _dbContext;
        public UniversitySeeder(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                /*
                if (_dbContext.Database.IsRelational())
                {
                    var pendingMigrations = _dbContext.Database.GetPendingMigrations();

                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        _dbContext.Database.Migrate();
                    }
                }
               */

                if (!_dbContext.Roles.Any())
                {
                    var rol = GetRoles();
                    _dbContext.Roles.AddRange(rol);
                    _dbContext.SaveChanges();

                }


                if (!_dbContext.Universities.Any())
                {
                    var uni = GetUniversities();
                    _dbContext.Universities.AddRange(uni);
                    _dbContext.SaveChanges();
                }


                if (!_dbContext.Students.Any())
                {
                    var stu = GetStudents();
                    _dbContext.Students.AddRange(stu);
                    _dbContext.SaveChanges();
                }

            }

        }


        private IEnumerable<Role> GetRoles() 
        {
            var rol = new List<Role>
            {
                new Role()
                {
                    Name="Student"
                },
                new Role()
                {
                    Name="Teacher"
                },
                new Role()
                {
                    Name="Admin"
                },
            };


            return rol;
        }
        private IEnumerable<Student> GetStudents()
        {
            var stu = new List<Student>
            {
                new Student()
                                {
                                    Title="Mgr",
                                    FirstName="Tomek",
                                    LastName="Kołodzejek",
                                    IndexNumber=209603,
                                    PESEL="0541518441",
                                    Specialization="Stara Bartka",
                                    YearOfStudies=4,
                                    Address = new Address()
                                    {

                                    }

                                },
                new Student()
                                {
                                    Title="Inż",
                                    FirstName="Kris",
                                    LastName="Rumucki",
                                    IndexNumber=79796,
                                    PESEL="0194918441",
                                    Specialization="Twoja Stara",
                                    YearOfStudies=1,
                                    Address = new Address()
                                    {

                                    }

                                },
                new Student()
                                {
                                    Title="Inż",
                                    FirstName="Krzysztof",
                                    LastName="Góral",
                                    IndexNumber=109609,
                                    PESEL="00222804075",
                                    Specialization="Back End dotnet",
                                    YearOfStudies=4,
                                    Address = new Address()
                                    {

                                    }

                                },
                                new Student()
                                {
                                    Title="Inż",
                                    FirstName="Kornel",
                                    LastName="Gołebiewski",
                                    IndexNumber=109606,
                                    PESEL="0194918441",
                                    Specialization="Twoja Stara",
                                    YearOfStudies=4,
                                    Address = new Address()
                                    {

                                    }

                                }

            };
            return stu;

        } 
            
            
        private IEnumerable<University> GetUniversities()
        {
            var uni = new List<University>()
            {
                new University()
                {
                    Name= "Politechnika Białystocka",
                    Description = "Bialystok University of Technology",
                    Type = "Technology",
                    ContactEmail = "pb.edu.pl",
                    ContactNumber = "85 746 90 01",
                    Rektor = new Teacher()
                    {
                        Title="prof. dr hab. inż.",
                        FirstName="Marta",
                        LastName="Kosior-Kazberuk"
                    },
                    Departments = new List<Department>
                    {
                        new Department()
                        {
                            Name="Informatyka",
                            Students = new List<Student>
                            {
                                new Student()
                                {

                                }
                            },
                            Teachers = new List<Teacher>
                            {
                                new Teacher()
                                {

                                }
                            }

                        }
                    },
                    Address = new Address()
                    {
                        City="Białystock",
                        Street="Wiejska 45a",
                        PostalCode="15-351"
                    }

                },
                new University()
                {
                    Name= "Politechnika Warszawska",
                    Description = "Warsaw University of Technology",
                    Type = "Technology",
                    ContactEmail = "warsaw.edu.pl",
                    ContactNumber = " 22 234 72 11",
                    Rektor = new Teacher()
                    {
                        Title="prof. dr hab. inż.",
                        FirstName="Krzysztof",
                        LastName="Zaremba"
                    },
                    Departments = new List<Department>
                    {
                        new Department()
                        {
                            Name="Informatyka",
                            Students = new List<Student>
                            {
                                
                            },
                            Teachers = new List<Teacher>
                            {
                                new Teacher()
                                {

                                }
                            }

                        }
                    },
                    Address = new Address()
                    {
                        City="Warszawa",
                        Street="plac Politechniki 1",
                        PostalCode="00-661"
                    }

                },
                new University()
                {
                    Name= "Politechnika Wrocławska",
                    Description = "Wroclaw University of Technology",
                    Type = "Technology",
                    ContactEmail = "pwr.edu.pl",
                    ContactNumber = "71 320 29 05",
                    Rektor = new Teacher()
                    {
                        Title="prof. dr hab. inż.",
                        FirstName="Arkadiusz",
                        LastName="Wójs"
                    },
                    Departments = new List<Department>
                    {
                        new Department()
                        {
                            Name="Architektura",
                            Students = new List<Student>
                            {
                                
                            },
                            Teachers = new List<Teacher>
                            {
                                new Teacher()
                                {

                                }
                            }

                        }
                    },
                    Address = new Address()
                    {
                        City="Wrocław",
                        Street="wybrzeże Stanisława Wyspiańskiego 279",
                        PostalCode="50-370"
                    }

                },
                new University()
                {
                    Name= "Politechnika Poznańska",
                    Description = "Poznań University of Technology",
                    Type = "Technology",
                    ContactEmail = "study@put.poznan.pl",
                    ContactNumber = "61 665 35 37",
                    Rektor = new Teacher()
                    {
                        Title="prof. dr hab. inż.",
                        FirstName="Teofil",
                        LastName="Jesionowski"
                    },
                    Departments = new List<Department>
                    {
                        new Department()
                        {
                            Name="Elektyka",
                            Students = new List<Student>
                            {
                               
                            },
                            Teachers = new List<Teacher>
                            {
                                new Teacher()
                                {

                                }
                            }

                        }
                    },
                    Address = new Address()
                    {
                        City="Poznań",
                        Street="Plac Marii Skłodowskiej-Curie 5",
                        PostalCode="60-965"
                    }

                },

            };


            return uni;
        }
        

    }
}
