<script setup>
    import API from '../services/api.js';
    import { createToDoItem } from '../factories/toDoFactory.js';
    import { createProvider } from '../services/ToDoProviderFactory';
    import { ref, onMounted, watch } from 'vue';
    import { debounce } from 'lodash';

    // Pick your provider factory pattern
    const providerType = ref('api'); // 'api' or 'local'
    const toDoProvider = ref(createProvider(providerType.value));

    watch(providerType, (newType) => {
        toDoProvider.value = createProvider(newType);
        getToDoItems();
    });

    // Search functionality
    const searchTerm = ref('');

    const searchToDoItems = async () => {
        const res = await toDoProvider.value.getToDoItems(searchTerm.value);
        toDoItems.value = Array.isArray(res.data) ? res.data : [];
    };

    watch(searchTerm, debounce(() => {
        getToDoItems();
    }, 300))

    const toDoItems = ref([]);
    const newToDo = ref('');
    const isUrgent = ref(false);
    const showForm = ref(false);
    const isEditing = ref(false);
    const editingItem = ref(null);
    


    const getToDoItems = async () => {
        //const res = await API.get('/todoitems');
        const res = await toDoProvider.value.getToDoItems(searchTerm.value);

        // Check if the response is an array or an object with data
        const items = Array.isArray(res)
            ? res
            : Array.isArray(res.data)
            ? res.data
            : [];
        
        console.log('GET /todoitems response:', items);
        toDoItems.value = items;
    };

    const addToDo = async () => {
        if (newToDo.value.trim() === '') return;

        const newItem = createToDoItem({
            title: newToDo.value,
            isUrgent: isUrgent.value,
        });

        try {
            console.log('Sending:', newItem);
            //const res = await API.post('/todoitems', newItem);
            const res = await toDoProvider.value.addToDo(newItem);
            console.log('Added:', res.data);
            getToDoItems();
            resetForm();
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

        const updatedItem = {
            ...editingItem.value,
            ...createToDoItem({
            title: newToDo.value,
            isUrgent: isUrgent.value
            })
        };

        try {
            await toDoProvider.value.editToDo(updatedItem);
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
        await toDoProvider.value.deleteToDo(toDoItem);
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
        <div>
            <select v-model="providerType">
                <option value="api">API Provider</option>
                <option value="local">LocalStorage Provider</option>
            </select>

            <button @click="showForm ? resetForm() : showForm = true" class="add-task-button">
                {{ showForm ? 'Cancel' : 'Add Task' }}
            </button>

            <input
                type="text"
                v-model="searchTerm"
                placeholder="Search..."
                class="search-input"
            />
        </div>
        

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