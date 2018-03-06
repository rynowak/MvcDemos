USE ApiControllerSample

INSERT INTO dbo.Categories (Name)
VALUES ('Dogs'), ('Cats'), ('Rabbits'), ('Lions')

INSERT INTO dbo.Pets (Age, CategoryId, HasVaccinations, Name, Status)
SELECT 1, Id, 1, Name + '1', 'available' FROM dbo.Categories;

INSERT INTO dbo.Pets (Age, CategoryId, HasVaccinations, Name, Status)
SELECT 1, Id, 1, Name + '2', 'available' FROM dbo.Categories;

INSERT INTO dbo.Pets (Age, CategoryId, HasVaccinations, Name, Status)
SELECT 1, Id, 1, Name + '3', 'available' FROM dbo.Categories;

INSERT INTO dbo.Tags (Name, PetId)
SELECT 'Tag1', ID FROM dbo.Pets

INSERT INTO dbo.Images ([Url], PetId)
SELECT [Url] = 'http://example.com/pets/' + CAST(Id as varchar) + '_1.png', ID from dbo.Pets 