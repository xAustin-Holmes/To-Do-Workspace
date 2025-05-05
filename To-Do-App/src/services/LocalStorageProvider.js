import IToDoProvider from './IToDoProvider';

const STORAGE_KEY = 'todos';

export default class LocalStorageProvider extends IToDoProvider {
    getToDoItems() {
        return JSON.parse(localStorage.getItem(STORAGE_KEY)) || [];
    }

    addToDo(item) {
        const items = this.getToDoItems();
        item.id = Date.now();
        items.push(item);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
        return item;
    }

    editToDo(item) {
        const items = this.getToDoItems().map(i => i.id === item.id ? item : i);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
    }

    deleteToDo(item) {
        const items = this.getToDoItems().filter(i => i.id !== item.id);
        localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
    }
}