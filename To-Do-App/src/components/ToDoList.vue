<script setup>
    import API from '../services/api.js';
    import { ref, onMounted } from 'vue';

    const toDoItems = ref([]);
    const newToDo = ref('');

    const getToDoItems = async () => {
        const res = await API.get('/todoitems');
        console.log('GET /todoitems response:', res.data);
        toDoItems.value = res.data;
    };

    const addToDo = async () => {
        if (newToDo.value.trim() === '') return;

        try {
            console.log('Sending:', {
                title: newToDo.value,
                isUrgent: true,
                isCompleted: false,
                createdAt: new Date().toISOString(),
            });

            const res = await API.post('/todoitems', {
                title: newToDo.value,
                isUrgent: true,
                isCompleted: false,
                createdAt: new Date().toISOString(),
            });

            console.log('Added:', res.data);
            getToDoItems();
            newToDo.value = '';
        } catch (err) {
            console.error('Add failed:', err);
            alert('Failed to add task: ' + (err.response?.data || err.message));
        }
    };

    const editToDo = async (toDo) => {
        try {
            await API.put(`/todoitems/${toDo.id}`, toDo);
            console.log('Updated:', toDo); // logging the updated todo item
            //getToDoItems();
        } catch (err) {
            console.error('Error updating todo:', err.response?.data || err.message);
        }
    };

    const deleteToDo = async (toDoItem) => {
        await API.delete(`/todoitems/${toDoItem.id}`);
        // CHANGE ***************
        getToDoItems();
        
    };

    onMounted(() => {
        getToDoItems();
        console.log("Component mounted");
    });

</script>

<template>
    <div>
        <h1>
            My To-Do List
        </h1>

        <form @submit.prevent="addToDo">
            <input v-model="newToDo" placeholder="New task" />
            <button>Add</button>
        </form>

        <ul>
            <li v-for="toDoItem in toDoItems" :key="toDoItem.id">
                <input type="checkbox" v-model="toDoItem.isCompleted" @change="editToDo(toDoItem)" />
                {{ toDoItem.title }}
                <button @click="deleteToDo(toDoItem)">Delete</button>
            </li>
        </ul>

        <div v-if="toDoItems.length === 0">
            No tasks available.
        </div>
    </div>
</template>

<style>

</style>