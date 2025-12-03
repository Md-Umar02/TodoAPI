// NOTE: You MUST replace this with the actual URL and endpoint of your running TodoAPI.
// For a typical .NET Core Todo API, 'api/todoitems' is common.
const BASE_URL = 'http://localhost:5211/api/todo'; 

document.addEventListener('DOMContentLoaded', () => {
    fetchTodos();

    document.getElementById('addTodoBtn').addEventListener('click', addTodo);
    // Allow adding by pressing 'Enter' in the input field
    document.getElementById('todoInput').addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            addTodo();
        }
    });
});

// --- API Interaction Functions ---

/**
 * Fetches all todos and renders them to the list.
 */
async function fetchTodos() {
    const todoListElement = document.getElementById('todoList');
    todoListElement.innerHTML = ''; // Clear existing list

    try {
        const response = await fetch(BASE_URL);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const todos = await response.json();
        
        todos.forEach(todo => {
            renderTodoItem(todo);
        });

    } catch (error) {
        console.error("Could not fetch todos:", error);
        todoListElement.innerHTML = `<li>Error loading tasks. Check the BASE_URL and ensure the API is running (${BASE_URL}).</li>`;
    }
}

/**
 * Creates a new todo item via POST request.
 */
async function addTodo() {
    const inputElement = document.getElementById('todoInput');
    const name = inputElement.value.trim();

    if (name === '') return;

    try {
        const newTodo = {
            // The property names must match your C# model (Name, IsComplete)
            name: name,
            isComplete: false
        };

        const response = await fetch(BASE_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newTodo)
        });

        if (!response.ok) {
             // Attempt to log server-side error message if available
             const errorText = await response.text();
             throw new Error(`Failed to add task (Status: ${response.status}): ${errorText}`);
        }

        // Successfully added, clear input and refresh list
        inputElement.value = '';
        fetchTodos(); 

    } catch (error) {
        console.error("Could not add todo:", error);
        alert(`Failed to add task. See console for details. Error: ${error.message}`);
    }
}

/**
 * Toggles the 'isComplete' status of a todo via PATCH request.
 */
async function toggleCompletion(id, isComplete) {
    try {
        const updatedTodo = {
            // Only send the property that changes (IsComplete)
            isComplete: !isComplete
        };

        const response = await fetch(`${BASE_URL}/${id}`, {
            method: 'PATCH', // Assuming your API supports PATCH for partial update
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedTodo)
        });

        if (!response.ok) {
            throw new Error(`Failed to update task (Status: ${response.status})`);
        }

        // Update successful, refresh list
        fetchTodos();

    } catch (error) {
        console.error("Could not toggle completion:", error);
        alert(`Failed to update task. See console for details. Error: ${error.message}`);
    }
}

/**
 * Deletes a todo item via DELETE request.
 */
async function deleteTodo(id) {
    if (!confirm('Are you sure you want to delete this task?')) return;

    try {
        const response = await fetch(`${BASE_URL}/${id}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new Error(`Failed to delete task (Status: ${response.status})`);
        }

        // Delete successful, refresh list
        fetchTodos();

    } catch (error) {
        console.error("Could not delete todo:", error);
        alert(`Failed to delete task. See console for details. Error: ${error.message}`);
    }
}


// --- Rendering Function ---

/**
 * Creates and appends an individual list item (LI) to the DOM.
 */
function renderTodoItem(todo) {
    const todoListElement = document.getElementById('todoList');

    const li = document.createElement('li');
    li.className = todo.isComplete ? 'completed' : '';

    const taskInfo = document.createElement('div');
    taskInfo.className = 'task-info';

    const checkbox = document.createElement('input');
    checkbox.type = 'checkbox';
    checkbox.checked = todo.isComplete;
    checkbox.addEventListener('change', () => {
        // Pass the item ID and current completion status to the toggle function
        toggleCompletion(todo.id, todo.isComplete);
    });

    const taskText = document.createElement('span');
    taskText.className = 'task-text';
    taskText.textContent = todo.name; 

    taskInfo.appendChild(checkbox);
    taskInfo.appendChild(taskText);

    const deleteBtn = document.createElement('button');
    deleteBtn.className = 'delete-btn';
    deleteBtn.textContent = 'Delete';
    deleteBtn.addEventListener('click', () => {
        deleteTodo(todo.id);
    });

    li.appendChild(taskInfo);
    li.appendChild(deleteBtn);
    todoListElement.appendChild(li);
}