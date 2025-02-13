# DealSpot

## About

Aplikacja jest projektem Web API mającym być w zamyśle api dla strony pozwalającej negocjować cenę wystawionego produktu na sprzedaż.

#### Kluczowe funkcje:

- **Dodawanie nowych produktów**: Umożliwia dodanie nowych produktów do systemu.
- **Pobieranie listy produktów**: Umożliwia pobranie listy dostępnych produktów.
- **Dodawanie negocjacji**: Umożliwia rozpoczęcie nowej negocjacji na podstawie zaproponowanej ceny.
- **Pobieranie listy negocjacji**: Umożliwia pobranie listy wszystkich negocjacji.
- **Pobieranie jednej negocjacji**: Umożliwia pobranie szczegółowych informacji o jednej negocjacji.
- **Akceptowanie ceny**: Umożliwia zaakceptowanie zaproponowanej ceny dla negocjacji.
- **Odrzucenie ceny**: Umożliwia odrzucenie ceny zaproponowanej dla negocjacji.
- **Anulowanie negocjacji**: Umożliwia anulowanie negocjacji po przekroczeniu określonego wcześniej limitu prób lub limitu czasowego.
- **Zaproponowanie nowej ceny**: Umożliwia zaproponowanie nowej ceny po uprzednim odrzuceniu wcześniej zaproponowanej ceny.

---

## API Description

### Product

#### `GET api/products`:

Służy do pobierania listy produktów.

##### Response:

- **200 OK**: Zwraca listę dostępnych zasobów w formacie JSON.
- **204 No Content**: Brak dostępnych zasobów do zwrócenia.

#### `GET api/products/{id}`

Służy do pobierania szczegółowych informacji na temat produktu o określonym identyfikatorze.

##### Parameters:

- **id**: Unikalny identyfikator zasobu, którego dotyczy zapytanie.

##### Response:

- **200 OK**: Zwraca szczególy zasobu o uprzednio podanym identyfikatorze w formacie JSON.
- **404 Not Found**: Brak dostępnego zasobu o podanym identyfikatorze.

#### `POST api/products:`

Służy do tworzenia produktu.

##### Body:

- **Product**: JSON produktu.

```json
{
  "name": string,
  "price": int
}
```

##### Response:

- **201 Created**: Informuje o pomyślnym stworzeniu zasobu oraz przesyła szczegóły tego zasobu.
- **400 Bad Request**: Informuje o niepoprawnych danych wejściowych.

---

### Negotiation

#### `GET api/negotiations`:

Służy do pobierania listy negocjacji.

##### Response:

- **200 OK**: Zwraca listę dostępnych zasobów w formacie JSON
- **204 No Content**: Brak dostępnych zasobów do zwrócenia.

#### `GET api/negotiations/{id}`:

Służy do pobierania szczegółowych informacji na temat negocjacji o określonym identyfikatorze.

##### Parameter:

- **id**: Unikalny identyfikator którego dotyczy zapytanie.

##### Response:

- **200 OK**: Zwraca szczególy zasobu o uprzednio podanym identyfikatorze w formacie JSON.
- **404 Not Found**: Brak dostępnego zasobu o podanym identyfikatorze.

#### `POST api/negotiations`:

Służy do stworzenia negocjacji na podstawie podanej ceny.

##### Body:

- **proposedPrice**: Proponowana cena jako double

```json
double
```

##### Response:

- **201 Created**: Informuje o pomyślnym stworzeniu zasobu na podstawie podanej ceny i zwraca pełne szczegóły dotyczące tego zasobu.
- **404 Not Found**: Brak dostępnego zasobu (produktu) o podanym identyfikatorze.

#### `POST api/negotiations/{id}`:

Służy do zmiany zaproponowanej ceny w negocjacji o podanym identyfikatorze.

##### Parameter:

- **id**: Unikalny identyfikator którego dotyczy zapytanie.

##### Body:

- **proposedPrice**: Proponowana cena jako double

```json
double
```

##### Response:

- **200 OK**: Informuje o pomyślnym zaproponowaniu nowej ceny
- **400 Bad Request**: Informuje o przekroczeniu ilości prób, czasu, jeśli zaproponowana cena była równa poprzednio zaproponowanej lub jeśli próbuje się aktualizować cenę anulowanej negocjacji.
- **404 Not Found**: Brak dostępnego zasobu o podanym identyfikatorze.

#### `POST api/negotiations/{id}/accept`:

Służy do zaakceptowania ceny w negocjacji o podanym identyfikatorze.

##### Parameter:

- **id**: Unikalny identyfikator którego dotyczy zapytanie

##### Response:

- **200 OK**: Informuje o pomyślnym zakceptowaniu ceny oraz zmianie statusu na 'PriceAccepted'.
- **404 Not Found**: Brak dostępnego zasobu o podanym identyfikatorze.

#### `POST api/negotiations/{id}/reject`:

Służy do odrzucenia ceny w negocjacji o podanym identyfikatorze.

##### Parameter:

- **id**: Unikalny identyfikator którego dotyczy zapytanie

##### Response:

- **200 OK**: Informuje o pomyślnym odrzuceniu ceny oraz zmianie statusu na 'PriceRejected'.
- **404 Not Found**: Brak dostępnego zasobu o podanym identyfikatorze.

#### `POST api/negotiations/{id}/cancel`:

Służy do anulowana negocjacji o podanym identyfikatorze.

##### Parameter:

- **id**: Unikalny identyfikator którego dotyczy zapytanie

##### Response:

- **200 OK**: Informuje o pomyślnej zmianie statusu na 'NegotiationCancelled'
- **404 Not Found**: Brak dostępnego zasobu o podanym identyfikatorze.

---

###### Ten projekt został stworzony na potrzeby rekrutacji.
