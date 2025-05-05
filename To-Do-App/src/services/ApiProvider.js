import IToDoProvider from './IToDoProvider';
import API from './api';

export default class ApiProvider extends IToDoProvider {
    async getToDoItems(searchTerm = '') {
        const url = searchTerm
            ? `/todoitems?search=${encodeURIComponent(searchTerm)}`
            : '/todoitems';
        return (await API.get(url)).data;
    }

    async addToDo(item) { return (await API.post('/todoitems', item)).data; }

    async editToDo(item) { return await API.put(`/todoitems/${item.id}`, item); }
    
    async deleteToDo(item) { return await API.delete(`/todoitems/${item.id}`); }
}