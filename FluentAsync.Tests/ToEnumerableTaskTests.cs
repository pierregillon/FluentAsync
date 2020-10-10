using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace FluentAsync.Tests
{
    public class ToEnumerableTaskTests
    {
        [Fact]
        public async Task Transform_a_task_of_list_into_a_task_of_enumerable()
        {
            var collection = new List<string> { SomeRandomString(), SomeRandomString(), SomeRandomString() };

            var element = await Task.FromResult(collection).ToEnumerableTask();

            element.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public async Task Transform_a_task_of_array_into_a_task_of_enumerable()
        {
            var collection = new[] { SomeRandomString(), SomeRandomString(), SomeRandomString() };

            var element = await Task.FromResult(collection).ToEnumerableTask();

            element.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public async Task Transform_a_task_of_readonly_collection_into_a_task_of_enumerable()
        {
            IReadOnlyCollection<string> collection = new[] { SomeRandomString(), SomeRandomString(), SomeRandomString() };

            var element = await Task.FromResult(collection).ToEnumerableTask();

            element.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public async Task Transform_a_task_of_readonly_list_into_a_task_of_enumerable()
        {
            IReadOnlyList<string> collection = new[] { SomeRandomString(), SomeRandomString(), SomeRandomString() };

            var element = await Task.FromResult(collection).ToEnumerableTask();

            element.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public async Task Transform_a_task_of_hashset_into_a_task_of_enumerable()
        {
            var hashset = new HashSet<string> { SomeRandomString(), SomeRandomString(), SomeRandomString() };

            var element = await Task.FromResult(hashset).ToEnumerableTask();

            element.Should().BeEquivalentTo(hashset);
        }

        [Fact]
        public async Task Transform_a_task_of_icollection_into_a_task_of_enumerable()
        {
            ICollection<string> collection = new[] { SomeRandomString(), SomeRandomString(), SomeRandomString() };

            var element = await Task.FromResult(collection).ToEnumerableTask();

            element.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public async Task Transform_a_task_of_collection_into_a_task_of_enumerable()
        {
            var collection = new Collection<string> { SomeRandomString(), SomeRandomString(), SomeRandomString() };
            
            var element = await Task.FromResult(collection).ToEnumerableTask();

            element.Should().BeEquivalentTo(collection);
        }

        private static string SomeRandomString() => Guid.NewGuid().ToString();
    }
}