# REMIT Submit API response codes and messages

- **Endpoint:** `POST /account/v2/remit/submit-api`
- **Headers:** `Authorization: Bearer <token>`, `Content-Type: application/xml`
- **Body:** a valid XML file, up to 128KB

## Success responses

| HTTP status | Message | Meaning |
| --- | --- | --- |
| 202 | `Accepted` | The notification was accepted for processing. |

## Error responses

| HTTP status | Message | Meaning |
| --- | --- | --- |
| 400 | `A non-empty request body is required` | Empty body. |
| 400 | `Invalid REMIT XML document root element` | Schema version not supported. |
| 400 | `<Validation failure reason and location>` | The XML failed schema validation — the message names the specific failure and location. |
| 400 | `<field1> must be earlier than <field2>` | Invalid date range in the notification. |
| 400 | `The submission is for a date/time in the past.` | A date/time field in the notification is in the past. |
| 401 | `Unauthorised` | Not authenticated (missing/invalid bearer token). |
| 401 | `User email not obtainable` | This email is not associated with an Elexon account. |
| 403 | `Forbidden` | This email does not have permission to submit REMIT notifications. |
| 403 | `You are not authorised to submit a notification for this asset` | Your organisation isn't registered as an authorised party for this asset. |
| 413 | `Payload Too Large` | Request body must be at most 128KB. |
| 415 | `Unsupported MediaType` | `Content-Type` must be `application/xml` or `text/xml`. |
| 500 | `Something Went Wrong` | Internal server error. |
| 503 | `Unable to process your request at this time. Please try again later.` | The API is temporarily unavailable — safe to retry later. |
