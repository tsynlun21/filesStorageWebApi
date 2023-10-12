# filesStorageWebApi
Написано на C#, NET 6.0, ASP.NET Core | Web API, Entity Framework, PostgreSQL, Docker

# Документация

## POST /Users/user

Метод добавления в базу данных пользователя, с указанным юзернеймом.

## PUT /Users/user/set-current/{id}

Метод выбора пользователя из базы данных и установкой его действующим лицом в рамках текущего скоупа.

## POST /Files/upload

Метод загрузки n-количества файлов в базу данных. 

**ВАЖНО**

Если в базе данных отсутсвует информация о пользователях, то загрузить файл не получится, т.к не будет информации о владельце. В этом случае будет выведено соотвествующее сообщение.

## GET /Files/download

Метод скачивания n-количества файлов из базы данных по Guid файлов.

**ВАЖНО** 

Скачать можно только файлы, владелец которых текущий пользователь. При скачивании >1 файла они будут предложены к загрузке в архиве.

## GET /Files/files

Метод вывода информации о имеющихся файлах в базе данных для текущего пользователя.

Ответ имеет вид 
```
{
  "id": # GUID файла
  "fileName": # Имя файла
  "ownerId": # Id владельца файла
}
```
## GET /Files/progress/{guid}

Метод вывода прогресса загрузки файла в базу данных

## GET /Files/link

Метод генерации одноразовой ссылки на скачивание файла.

## GET /Files/download/{link}

Метод скачивания файла по одноразовой ссылке. Этой ссылкой может пользоваться любой пользователь

