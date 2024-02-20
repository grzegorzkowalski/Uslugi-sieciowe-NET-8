## Zadanie 2

1. Do kontrolera `PostsController` dodaj dekorator `[Authorize]`.
1. Od tego momentu skorzystanie z metod tego kontrolera będzie wymagało tokena JWT.
1. Zezwól na pobieranie postów bez konieczności podpisywania requestu tokenem.
1. Dodaj `RegisterController`, który będzie pozwalał uwierzytelnionym użytkownikom dodawać nowe osoby do bazy.
1. Dla uproszczenia na tym etapie możemy zakładać, że każdy zarejestrowany użytkownik ma rolę `Admin`.
1. Przetestuj czy możesz się teraz zalogować i skorzystać z chronionych kontolerów (dodaj nowy post) 
i metod za pomocą wcześniej utworzonego konta w bazie danych.