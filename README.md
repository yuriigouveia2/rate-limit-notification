Markdown
## RateLimitNotification API

**Project Name:** rate-limit-notification

**Description:**

This project provides an API for sending notifications to users. It allows single or bulk notifications with different types (e.g., marketing, status). The API utilizes rate limiting to prevent overwhelming notification systems.

**Features:**

* Send single notifications to users
* Send multiple notifications in a batch
* Enforce rate limits on notification requests

**Getting Started**

**Running the Application with Docker Compose**

This project can be run using Docker Compose. Here are the steps:

**Prerequisites:**

* Docker installed (https://www.docker.com/)
* Docker Compose installed (https://docs.docker.com/compose/install/)
* A code editor with Docker Compose support (optional)

**Instructions:**

1. **Clone the Repository (if not already done):**

   ```bash
   git clone [https://github.com/yuriigouveia2/rate-limit-notification.git](https://github.com/yuriigouveia2/rate-limit-notification.git)

2. **Navigate to the project directory:**
    ```bash
    cd RateLimitNotification

3. **Build and Run the Application with Docker Compose:**
    ```bash
    docker-compose up

## Running the Application with Docker Compose (continued)

**Accessing the API:**

By default, the application will be accessible on port `5001` of your machine (replace with the actual port if you modified it in the `docker-compose.yml` file). You can use tools like Postman or curl to send requests to the API endpoints. Swagger is also availabe on [http://localhost:5001/swagger](http://localhost:5001/swagger) and there are payloads documentations in it (The swagger is only present with the variable `ASPNETCORE_ENVIRONMENT` setted to `Development`).

**Configuration (Optional):**

The `docker-compose.yml` file defines the environment variables used by the application. You can customize these variables by editing the file:

* `ConnectionStrings__Cache`: This should be set to the connection string for your Redis server. By default, it points to the `redis` service running in the Docker Compose configuration.
* `ASPNETCORE_ENVIRONMENT`: This variable specifies the environment your application runs in. It's set to `Development` by default, but you can change it to `Production` for a production environment.

## Rate Limiting in Api and Service Layers

The API implements rate limiting using Redis, a popular in-memory data store. This approach ensures fast and scalable rate limit checks. A custom `RateLimitService` is responsible for managing rate limits. Here's how it works:

**Filter Integration:** The `RateLimitSingleNotificationFilterAttribute` and `RateLimitMultipleNotificationsFilterAttribute` are applied to API endpoints that handle single and multiple notifications requests. These filters intercepts the request execution pipeline before the controller action is invoked.

**Rate Limit Check:** The filters retrieve the `IRateLimitService` from the dependency injection container and extracts the `UserId` and `NotificationType` from the request body (in case of the use of the endpoint `/multiple` there will be a list of these fields). It then calls the `CanNotify` method on the `IRateLimitService` to check if the user has reached the maximum allowed notifications for the specific notification type.

**Redis Interaction (Implemented in IRateLimitService):**

- The `IRateLimitService` interacts with Redis to perform the rate limit check. It likely uses Redis key-value for each user-notification type combination.

**Action Execution or Error Response:**

- If the rate limit check succeeds (`CanNotify` returns true), the filter allows the request to proceed by calling the `next` delegate in the pipeline. The controller action responsible for sending the notification is then executed.

- If the rate limit check fails (`CanNotify` returns false), the filter aborts the request and returns a `BadRequestObjectResult` with an error message indicating the user has exceeded the notification limit for the specific type.

## Strategy Pattern for Notification Rate Limiting

The code contains the use of the Strategy Pattern for handling notification rate limits based on notification types. Here's a breakdown:

**Interfaces:**

* `INotificationTypeStrategy`: This interface defines the core methods for notification type strategies:
    - `CanNotify(int currentNotificationCount)`: This method checks if a notification can be sent based on the current notification count (retrieved from Redis).
    - `GetTtl()`: This method retrieves, from environment variables, the time-to-live (TTL) value for the specific notification type, which determines the expiration time for rate limit entries in Redis.

**Strategy Resolver:**

* `NotificationTypeStrategyResolver`: This class is responsible for resolving the appropriate strategy based on the notification type:
    - It receives a `RateLimitConfiguration` object during initialization, containing TTL values for different notification types.
    - The `Get(string notificationType)` method uses a `switch` statement to map notification types to concrete strategy implementations:
        - `NewsStrategy` handle news notifications with a specific TTL and logic for allowing notifications based on the count.
        - `StatusStrategy` handle status notifications with a different TTL.
        - `MarketingStrategy` handle marketing notifications.
    - If an unsupported notification type is encountered, a `NotSupportedException` is thrown.


## Redis Usage for Rate Limiting

This code snippet utilizes Redis Strings (key-value pairs) to manage rate limits for notification requests. Here's a breakdown of the approach:

**Data Structure and Key Format:**

- Redis Strings are used to store rate limit information, with a simple key-value structure.
- The key format is designed to identify individual rate limits for specific user-notification type combinations:

    ```
    user_id:notification_type:timestamp:{userId}:{notificationType}:{current_timestamp_in_ticks}
    ```

    - `user_id`: Unique identifier for the user.
    - `notification_type`: Type of notification (e.g., marketing, status).
    - `timestamp`: Represents a unique identifier within the key to differentiate entries for the same user-notification type combination.
    - `current_timestamp_in_ticks`: Uses `DateTime.Now.Ticks` for a high-precision expiration timestamp.

**Saving Rate Limits:**

The `SaveOnCache` method takes a `RateLimit` object with properties like `UserId`, `NotificationType`, and `Ttl` (time to live). Here's how it works:

1. **Key Construction:** A unique key is constructed using the provided user ID, notification type, and current timestamp in ticks. This ensures each user-notification type combination has its own dedicated key in Redis.
2. **Value Setting:** The value for the constructed key is typically set to `1`. The value itself is not significant. The key's presence with an expiration time serves the rate limiting purpose.
3. **Expiration Setting:** The method uses `expiry: rateLimit.Ttl` to set an expiration based on the `Ttl` property of the `RateLimit` object. The key automatically expires after the specified time window, effectively resetting the rate limit for that user-notification type combination.

**Getting Notification Count:**

The `GetNotificationCount` method retrieves the current notification count for a specific user and notification type. Here's the process:

1. **Pattern Construction:** It constructs a pattern for key retrieval using `user_id:notification_type:timestamp:{userId}:{notificationType}*`. The asterisk (*) acts as a wildcard to match any timestamp component within the key. This allows retrieving all keys associated with the specified user ID and notification type, regardless of the specific timestamp.
2. **Key Retrieval:** It retrieves all keys matching the pattern using `_server.Keys(database: _redis.Database, pattern: pattern)`. This fetches all keys for the user-notification type combination, potentially containing entries from previous time windows.
3. **Count Calculation:** It calculates the notification count by getting the length of the retrieved `notificationsKeys` array. This represents the total number of entries currently in Redis for the given user-notification type combination.

**Overall Approach:**

This code demonstrates a rate limiting approach using Redis Strings. Each user-notification type combination has its own dedicated key expiring after a predefined time window. The `GetNotificationCount` method retrieves all keys matching a pattern to determine the total count within the potentially combined time windows.
