# REMIT Submit API response codes and messages

- **Endpoint:** `POST /account/v2/remit/submit-api`
- **Headers:** `Authorization: Bearer <token>`, `Content-Type: application/xml`
- **Body:** a valid XML file, up to 128KB

## Success responses

| HTTP status | Message | Meaning |
| --- | --- | --- |
| 202 | `Accepted` | The notification was accepted for processing. |

## Error responses

| HTTP status | Message | Cause |
| --- | --- | --- |
| 400 | `A non-empty request body is required` | Empty body. |
| 400 | `Invalid REMIT XML document root element` | Schema version not supported. |
| 400 | `<Validation failure reason and location>` | The XML failed schema validation — the message names the specific failure and location. |
| 400 | `<field1> must be earlier than <field2>` | Invalid date range in the notification. |
| 400 | `The submission is for a date/time in the past.` | Submission date is in the past. |
| 401 | `Unauthorised` | Not authenticated (missing/invalid bearer token). |
| 401 | `User email not obtainable` | The caller's email isn't obtainable from Entra. |
| 403 | `Forbidden` | The token doesn't have the REMIT scope. |
| 403 | `Forbidden` | The token doesn't have the REMIT role. |
| 403 | `You are not authorised to submit a notification for this asset` | The caller isn't an authorised party for the asset in the notification. |
| 413 | `Payload Too Large` | Request body exceeds 128KB. |
| 415 | `Unsupported MediaType` | `Content-Type` isn't `application/xml`/`text/xml`. |
| 500 | `Something Went Wrong` | Internal server error. |
| 503 | `Unable to process your request at this time. Please try again later.` | The API is temporarily unavailable — safe to retry later. |
