<script setup>
    import API from '../services/api.js';
    import { ref, onMounted } from 'vue';

    const toDoItems = ref([]);
    const newToDo = ref('');
    const isUrgent = ref(false);
    const showForm = ref(false);
    const isEditing = ref(false);
    const editingItem = ref(null);

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
                isUrgent: isUrgent.value,
                isCompleted: false,
                createdAt: new Date().toISOString(),
            });

            const res = await API.post('/todoitems', {
                title: newToDo.value,
                isUrgent: isUrgent.value,
                isCompleted: false,
                createdAt: new Date().toISOString(),
            });

            console.log('Added:', res.data);
            getToDoItems();
            isUrgent.value = false;
            showForm.value = false;
            newToDo.value = '';
        } catch (err) {
            console.error('Error adding item:', err.response?.data || err.message);
        }
    };

    const beginEdit = (item) => {
        newToDo.value = item.title;
        isUrgent.value = item.isUrgent;
        editingItem.value = item;
        isEditing.value = true;
        showForm.value = true;
    };

    const editToDo = async () => {
        if (!editingItem.value) return;

        try {
            const updatedItem = {
                ...editingItem.value,
                title: newToDo.value,
                isUrgent: isUrgent.value,
            };

            await API.put(`/todoitems/${updatedItem.id}`, updatedItem);
            console.log('Updated:', updatedItem);
            // CHANGE *************** dont keep running the getToDoItems() in final
            getToDoItems();
            resetForm();
        } catch (err) {
            console.error('Error saving edit:', err.response?.data || err.message);
        }
    };

    const resetForm = () => {
        newToDo.value = '';
        isUrgent.value = false;
        showForm.value = false;
        isEditing.value = false;
        editingItem.value = null;
    };

    const deleteToDo = async (toDoItem) => {
        await API.delete(`/todoitems/${toDoItem.id}`);
        // CHANGE *************** dont keep running the getToDoItems() in final
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

        <button @click="showForm ? resetForm() : showForm = true" class="add-task-button">
            {{ showForm ? 'Cancel' : 'Add Task' }}
        </button>

        <div v-if="showForm" class="task-form">
            <form @submit.prevent="isEditing ? editToDo() : addToDo()">
                <input v-model="newToDo" placeholder="Task title" />
                <label>
                    <input type="checkbox" v-model="isUrgent" />
                    Urgent
                </label>
                <button type="submit">{{ isEditing ? 'Save' : 'Add' }}</button>
            </form>
        </div>

        <div v-if="toDoItems.length === 0">
            No tasks available.
        </div>

        <table v-else class="todo-table">
            <thead>
                <tr>
                <th>Completed</th>
                <th>Title</th>
                <th>Urgent?</th>
                <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="toDoItem in toDoItems" :key="toDoItem.id">
                <td>
                    <input
                    type="checkbox"
                    v-model="toDoItem.isCompleted"
                    @change="editToDo(toDoItem)"
                    />
                </td>
                <td>{{ toDoItem.title }}</td>
                <td>{{ toDoItem.isUrgent ? 'Yes' : 'No' }}</td>
                <td>
                    <button @click="beginEdit(toDoItem)">Edit</button>
                    <button @click="deleteToDo(toDoItem)">Delete</button>
                </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<style>
    .todo-container {
        max-width: 600px;
        margin: 0 auto;
        padding: 20px;
    }

    .add-task-button {
     margin-bottom: 10px;
    }

    .task-form {
        margin-bottom: 20px;
        border: 1px solid #ccc;
        padding: 10px;
        background: #f9f9f9;
    }

    .todo-table {
        width: 100%;
        border-collapse: collapse;
    }

    .todo-table th,
    .todo-table td {
        border: 1px solid #ccc;
        padding: 10px;
        text-align: left;
    }

    .todo-table th {
        background-color: #f5f5f5;
    }

    .empty-message {
        text-align: center;
        color: #666;
    }
</style>