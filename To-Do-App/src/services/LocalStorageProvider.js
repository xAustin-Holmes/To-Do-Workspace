import IToDoProvider from './IToDoProvider';

const STORAGE_KEY = 'todos';

export default class LocalStorageProvider extends IToDoProvider {
    getToDoItems(searchTerm = '') {
        const items = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]');
        const filteredItems = searchTerm.trim()
            ? items.filter(item =>
                item.title.toLowerCase().includes(searchTerm.toLowerCase())
            )
            : items;
        
        return { data: filteredItems };
    }

    addToDo(item) {
        const items = this.getToDoItems().data;
        item.id = Date.now();
        items.push(item);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
        return { data: item };
    }

    editToDo(item) {
        const items = this.getToDoItems().data.map(i => i.id === item.id ? item : i);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
        return { data: item };
    }

    deleteToDo(item) {
        const items = this.getToDoItems().data.filter(i => i.id !== item.id);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
        return { data: item };
    }
}