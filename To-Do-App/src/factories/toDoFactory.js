export function createToDoItem({ title, isUrgent }) {
    return {
        title: title.trim(),
        isUrgent: isUrgent,
        isCompleted: false,
        createdAt: new Date().toISOString(),
    };
}