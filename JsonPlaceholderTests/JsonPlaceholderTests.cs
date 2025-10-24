using System.Net;
using Xunit;
using RestSharp;
using FluentAssertions;
using Newtonsoft.Json;
using Bogus;

/**
 * Test suite for the JSONPlaceholder API
 */
namespace JsonPlaceholderTests
{
    /**
     * Class to maintain HTTP request tests for the /users endpoint
     */
    public class UsersAPITests
    {

        /* Obtain API data from JSONPlaceholder */
        private RestClient client;

        /* Bogus for fake User data */
        private Faker<User> userFaker;
        private Faker<Address> addressFaker;
        private Faker<Geo> geoFaker;
        private Faker<Company> companyFaker;

        /**
         * Set up the attributes necessary to complete the test cases
         */
        public UsersAPITests()
        {
            // Configure the URL of the API 
            client = new RestClient("https://jsonplaceholder.typicode.com");

            // Set up all the fakers for data generation for posts/puts
            userFaker = new Faker<User>()
                .RuleFor(user => user.name, x => x.Name.FullName())
                .RuleFor(user => user.username, x => x.Internet.UserName())
                .RuleFor(user => user.email, x => x.Internet.Email())
                .RuleFor(user => user.address, x => addressFaker.Generate())
                .RuleFor(user => user.phone, x => x.Phone.PhoneNumber())
                .RuleFor(user => user.website, x => x.Internet.DomainName())
                .RuleFor(user => user.company, x => companyFaker.Generate());

            addressFaker = new Faker<Address>()
                .RuleFor(address => address.street, x => x.Address.StreetName())
                .RuleFor(address => address.suite, x => x.Address.SecondaryAddress())
                .RuleFor(address => address.city, x => x.Address.City())
                .RuleFor(address => address.zipcode, x => x.Address.ZipCode())
                .RuleFor(address => address.geo, x => geoFaker.Generate());

            geoFaker = new Faker<Geo>()
                .RuleFor(geo => geo.lat, x => x.Address.Latitude().ToString())
                .RuleFor(geo => geo.lng, x => x.Address.Longitude().ToString());

            companyFaker = new Faker<Company>()
                .RuleFor(company => company.name, x => x.Company.CompanyName())
                .RuleFor(company => company.catchPhrase, x => x.Company.CatchPhrase())
                .RuleFor(company => company.bs, x => x.Company.Bs());
        }

        /**
         * Test getting the list of all stored users
         */
        [Fact]
        public async Task TestGetAllUsers()
        {
            // Get the list of Users
            RestRequest request = new RestRequest("users", Method.Get);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify the existence of the list and the size
            List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Content!);
            users.Should().NotBeNull();
            users.Should().HaveCountGreaterThan(0);
            users.Should().HaveCount(10);
        }

        /**
         * Test getting specific Users
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestGetUserById(int userId)
        {
            // Get the User by ID
            RestRequest request = new RestRequest($"users/{userId}", Method.Get);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify that the User and its attributes exist
            User user = JsonConvert.DeserializeObject<User>(response.Content!);
            user.Should().NotBeNull();
            user.id.Should().Be(userId);
            user.name.Should().NotBeNullOrEmpty();
            user.username.Should().NotBeNullOrEmpty();
            user.email.Should().NotBeNullOrEmpty();
            user.phone.Should().NotBeNullOrEmpty();
            user.website.Should().NotBeNullOrEmpty();

            user.address.Should().NotBeNull();
            user.address.street.Should().NotBeNullOrEmpty();
            user.address.suite.Should().NotBeNullOrEmpty();
            user.address.city.Should().NotBeNullOrEmpty();
            user.address.zipcode.Should().NotBeNullOrEmpty();
            user.address.geo.Should().NotBeNull();
            user.address.geo.lat.Should().NotBeNullOrEmpty();
            user.address.geo.lng.Should().NotBeNullOrEmpty();

            user.company.Should().NotBeNull();
            user.company.name.Should().NotBeNullOrEmpty();
            user.company.catchPhrase.Should().NotBeNullOrEmpty();
            user.company.bs.Should().NotBeNullOrEmpty();
        }

        /**
         * Test getting Posts made by a specific User
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestGetUserPosts(int userId)
        {
            // Get the Posts made by the User
            RestRequest request = new RestRequest($"users/{userId}/posts", Method.Get);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify that the Posts exist
            List<Post> posts = JsonConvert.DeserializeObject<List<Post>>(response.Content!);
            posts.Should().NotBeNull();
            posts.Should().HaveCountGreaterThan(0);
            posts.Should().HaveCount(10);
        }

        /**
         * Test getting Todos made by a specific User
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestGetUserTodos(int userId)
        {
            // Get the Todos made by the User
            RestRequest request = new RestRequest($"users/{userId}/todos", Method.Get);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify that the Todos exist
            List<Post> todos = JsonConvert.DeserializeObject<List<Post>>(response.Content!);
            todos.Should().NotBeNull();
            todos.Should().HaveCountGreaterThan(0);
            todos.Should().HaveCount(20);
        }

        /**
         * Test getting Albums made by a specific User
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestGetUserAlbums(int userId)
        {
            // Get the Albums made by the User
            RestRequest request = new RestRequest($"users/{userId}/albums", Method.Get);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify that the Albums exist
            List<Post> albums = JsonConvert.DeserializeObject<List<Post>>(response.Content!);
            albums.Should().NotBeNull();
            albums.Should().HaveCountGreaterThan(0);
            albums.Should().HaveCount(10);
        }

        /**
         * Test getting an invalid User
         */
        [Fact]
        public async Task TestGetInvalidUser()
        {
            // Try to get a User that is not present in the database
            RestRequest request = new RestRequest("users/11", Method.Get);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            // Test fails due to nature of the database, but in a working environment should pass
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /**
         * Test the valid creation of a new User and posting it to the API
         */
        [Fact]
        public async Task TestPostValidUser()
        {
            // Create a fake User to post
            User user = userFaker.Generate();

            RestRequest request = new RestRequest("users", Method.Post);
            request.AddJsonBody(user);

            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Check the response and make sure the contents of the User are correct
            User postedUser = JsonConvert.DeserializeObject<User>(response.Content!);
            postedUser.Should().NotBeNull();
            postedUser.id.Should().Be(11);
            HelperEquals(postedUser, user);

            /* Further testing to be performed in a working environment */

            /*
                // Check that the newly posted User can be obtained from the database
                request = new RestRequest($"users/{postedUser.id}", Method.Get);
                response = await client.ExecuteAsync(request);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                HelperEquals(postedUser, user);

                // Check for an updated list containing the newly posted User with an updated User list size
                request = new RestRequest("users", Method.Get);
                response = await client.ExecuteAsync(request);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Content!);
                users.Should().HaveCount(11);
            */
        }

        /**
         * Test the invalid creation of a new User and posting it
         */
        [Fact]
        public async Task TestPostInvalidUser()
        {
            // Create a fake User and set an attribute to something incorrect
            User user = userFaker.Generate();
            user.username = null;

            RestRequest request = new RestRequest("users", Method.Post);
            request.AddJsonBody(user);

            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            // Test fails due to inability to truly alter the database, but in a working environment should pass
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /**
         * Test the creation of a new User and posting it with an invalid endpoint
         */
        [Fact]
        public async Task TestPostValidUserInvalidEndpoint()
        {
            // Create a valid User, but attempt to post with an incorrect endpoint
            User user = userFaker.Generate();

            RestRequest request = new RestRequest("users/1", Method.Post);
            request.AddJsonBody(user);

            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            // Test fails due to the return of a 404 status. It would make more sense for this to return a 400
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /**
         * Test the valid update of an existing User
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestPutValidUser(int userId)
        {
            // Create a fake User and set the ID to one that already exists
            User user = userFaker.Generate();
            user.id = userId;

            RestRequest request = new RestRequest($"users/{userId}", Method.Put);
            request.AddJsonBody(user);

            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Check that the values were updated for the User
            User updatedUser = JsonConvert.DeserializeObject<User>(response.Content!);
            updatedUser.Should().NotBeNull();
            updatedUser.id.Should().Be(userId);
            HelperEquals(updatedUser, user);

            /* Further testing to be performed in a working environment */

            /*
                // Check that the updated User can be obtained from the database, and that the attributes have truly been updated
                request = new RestRequest($"users/{updatedUser.id}", Method.Get);
                response = await client.ExecuteAsync(request);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                updatedUser = JsonConvert.DeserializeObject<User>(response.Content!);
                updatedUser.Should().NotBeNull();
                updatedUser.id.Should().Be(userId);
                HelperEquals(updatedUser, user);

                // Check for an updated list containing the updated User with the same User list size
                request = new RestRequest("users", Method.Get);
                response = await client.ExecuteAsync(request);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Content!);
                users.Should().HaveCount(10);
            */
        }

        /**
         * Test the invalid update of an existing User
         */
        [Fact]
        public async Task TestPutInvalidUser()
        {
            // Create a fake User and set an attribute to something incorrect
            User user = userFaker.Generate();
            user.username = null;
            user.id = 1;

            RestRequest request = new RestRequest("users/1", Method.Put);
            request.AddJsonBody(user);

            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            // Test fails due to nature of the database, but in a working environment should pass
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Attempt to update a User that is not already present in the database
            user = userFaker.Generate();
            user.id = 11;

            request = new RestRequest("users/11", Method.Put);
            request.AddJsonBody(user);

            response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            // Test fails due to nature of the database, but in a working environment should pass
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /**
         * Test the valid patch of an existing User
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestPatchValidUser(int userId)
        {
            // Get the User by ID
            RestRequest request = new RestRequest($"users/{userId}", Method.Get);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            User user = JsonConvert.DeserializeObject<User>(response.Content!);

            string oldUsername = user.username;
            // Patch the retrieved User
            string newUsername = "username";
            var json = JsonConvert.SerializeObject(newUsername);
            request = new RestRequest($"users/{userId}", Method.Patch);
            request.AddJsonBody(json);

            response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Check that the value was updated for the User
            User updatedUser = JsonConvert.DeserializeObject<User>(response.Content!);
            updatedUser.Should().NotBeNull();
            updatedUser.id.Should().Be(userId);
            updatedUser.username.Should().Be(newUsername);
            updatedUser.username.Should().NotBe(oldUsername);

            /* Further testing to be performed in a working environment */

            /*
                // Check that the updated User can be obtained from the database, and that the attributes have truly been updated
                request = new RestRequest($"users/{updatedUser.id}", Method.Get);
                response = await client.ExecuteAsync(request);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                updatedUser = JsonConvert.DeserializeObject<User>(response.Content!);
                updatedUser.Should().NotBeNull();
                updatedUser.id.Should().Be(userId);
                HelperEquals(updatedUser, user);

                // Check for an updated list containing the updated User with the same User list size
                request = new RestRequest("users", Method.Get);
                response = await client.ExecuteAsync(request);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Content!);
                users.Should().HaveCount(10);
            */
        }

        /**
         * Test the valid deletion of an existing User
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task TestDeleteValidUser(int userId)
        {
            // Delete a selected User
            RestRequest request = new RestRequest($"users/{userId}", Method.Delete);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            /* Further testing to be performed in a working environment */

            /*
                // Check that the deleted User can no longer be obtained
                request = new RestRequest($"users/{userId}", Method.Get);
                Func<Task> action = async () => await client.GetAsync(request);
                await action.Should().ThrowAsync<HttpRequestException>();

                // Check for an updated list not containing the deleted User with the updated User list size
                request = new RestRequest("users", Method.Get);
                response = await client.ExecuteAsync(request);

                response.Should().NotBeNull();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                List<User> users = JsonConvert.DeserializeObject<List<User>>(response.Content!);
                users.Should().HaveCount(9);
            */
        }

        /**
         * Test the invalid deletion of a User
         */
        [Fact]
        public async Task TestDeleteInvalidUser()
        {
            // Attempt to delete a User with the ID of 11 (not in database)
            RestRequest request = new RestRequest("users/11", Method.Delete);
            RestResponse? response = await client.ExecuteAsync(request);

            response.Should().NotBeNull();
            // Test fails due to inability to truly alter the database, but in a working environment should pass
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /**
         * Helper method to test equality between 2 Users
         */
        private void HelperEquals(User user1, User user2)
        {
            user1.name.Should().Be(user2.name);
            user1.username.Should().Be(user2.username);
            user1.email.Should().Be(user2.email);
            user1.phone.Should().Be(user2.phone);
            user1.website.Should().Be(user2.website);

            user1.address.Should().NotBeNull();
            user1.address.street.Should().Be(user2.address.street);
            user1.address.suite.Should().Be(user2.address.suite);
            user1.address.city.Should().Be(user2.address.city);
            user1.address.zipcode.Should().Be(user2.address.zipcode);
            user1.address.geo.lat.Should().Be(user2.address.geo.lat);
            user1.address.geo.lng.Should().Be(user2.address.geo.lng);

            user1.company.Should().NotBeNull();
            user1.company.name.Should().Be(user2.company.name);
            user1.company.catchPhrase.Should().Be(user2.company.catchPhrase);
            user1.company.bs.Should().Be(user2.company.bs);
        }

        /**
         * DTOs for the User and its components
         */
        private class User
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string username { get; set; } = string.Empty;
            public string email { get; set; } = string.Empty;
            public Address address { get; set; } = new Address();
            public string phone { get; set; } = string.Empty;
            public string website { get; set; } = string.Empty;
            public Company company { get; set; } = new Company();
        }

        private class Address
        {
            public string street { get; set; } = string.Empty;
            public string suite { get; set; } = string.Empty;
            public string city { get; set; } = string.Empty;
            public string zipcode { get; set; } = string.Empty;
            public Geo geo { get; set; } = new Geo();
        }

        private class Geo
        {
            public string lat { get; set; } = string.Empty;
            public string lng { get; set; } = string.Empty;
        }

        private class Company
        {
            public string name { get; set; } = string.Empty;
            public string catchPhrase { get; set; } = string.Empty;
            public string bs { get; set; } = string.Empty;
        }

        /**
         * DTO for the Post
         */
        private class Post
        {

            public int userId { get; set; }
            public int id { get; set; }
            public string title { get; set; } = string.Empty;
            public string body { get; set; } = string.Empty;
        }

        /**
         * DTO for the Todo
         */
        private class Todo
        {

            public int userId { get; set; }
            public int id { get; set; }
            public string title { get; set; } = string.Empty;
            public Boolean completed { get; set; } = false;
        }

        /**
         * DTO for the Album
         */
        private class Album
        {

            public int userId { get; set; }
            public int id { get; set; }
            public string title { get; set; } = string.Empty;
        }
    }
}
