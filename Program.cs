using LibraryManagementSystem.Data;
using LibraryManagementSystem.repository;
using LibraryManagementSystem.Service;
using LibraryManagementSystem.view;

AppDbContext dbContext = new AppDbContext();
Repository repository = new Repository(dbContext);
Service service = new Service(repository);
View view = new View(service);

view.run();