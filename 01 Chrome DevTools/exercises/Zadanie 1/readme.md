## Zadanie 1 - Zapytania Rest API

1. Wykorzystaj poznany już serwis [https://jsonplaceholder.typicode.com/](https://jsonplaceholder.typicode.com/) i język
JavaScript do wykonania zapytań typu `POST`, `PUT`, `PATCH`, `DELETE` dla zasobu `/posts`.
3. Zaprezentuj analizę i różnice między poszczególnymi typami żądań i wynikami.

Możesz skorzystać z poniższych zapytań:

```js
//GET ALL
fetch("https://jsonplaceholder.typicode.com/posts")
    .then(res => res.json())
    .then(data => console.log(data));
```

```js
//GET ONE
fetch("https://jsonplaceholder.typicode.com/posts/5")
    .then(res => res.json())
    .then(data => console.log(data));
```

```js
// POST
const obj = {
        "userId": 5,
        "title": "Nowy tytuł",
        "body": "treść posta"
    };
	
fetch("https://jsonplaceholder.typicode.com/posts", 
    {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        }
        ,   
        body: JSON.stringify(obj)
    })
.then(res => res.json())
.then(data => console.log(data));
```

```js
// PUT
const obj = {
        "userId": 5,
        "title": "Całkiem nowy tytuł",
        "body": "treść posta"
    };
	
fetch("https://jsonplaceholder.typicode.com/posts/42",
    {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        }
        ,   
        body: JSON.stringify(obj)
    })
.then(res => res.json())
.then(data => console.log(data));
```

```js
// PATCH
const obj = {
        "title": "Zmieniamy tylko tytuł"
    };
	
fetch("https://jsonplaceholder.typicode.com/posts/42",
    {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json"
        }
        ,   
        body: JSON.stringify(obj)
    })
.then(res => res.json())
.then(data => console.log(data));
```

```js
// DELETE
fetch("https://jsonplaceholder.typicode.com/posts/42", 
    {
        method: "DELETE"
    })
.then(res => res.json())
.then(data => console.log(data));
```




