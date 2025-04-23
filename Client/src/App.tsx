import React, { useEffect, useState } from 'react';
import { Task } from './types/Task';
import TaskList from './components/task/TaskList';
import * as tasksApi from './api/task-service';

const App: React.FC = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [isAddRowVisible, setAddRowVisible] = useState(false);
  const [errorText, setErrorText] = useState('');

  useEffect(() => {
    fetchTasks();

    // set up polling
    const interval = setInterval(() => {
      fetchTasks();
    }, 5000);

    return () => {
      clearInterval(interval);
    };
  }, []);

  const handleUpdateTask = async (updatedTask: Task) => {
    try {
      const updated = await tasksApi.updateTask(updatedTask);
      setTasks(prevTasks => [...prevTasks.filter(x => x.id !== updated.id), updated]);
    } catch (err: any) {
      setErrorText(err.response?.data || "Update failed. Please try again.");
    }
  };

  const handleDeleteTask = async (id: string) => {
    setTasks(tasks.filter((task) => task.id !== id));
    await tasksApi.deleteTask(id).catch(err => setErrorText(err.response.data));
  };

  const handleAddTask = async (newTask: Task) => {
    await tasksApi.addTask(newTask).catch(err => setErrorText(err.response.data));
    setAddRowVisible(false);
    fetchTasks();
  };

  const toggleAddRow = () => {
    setAddRowVisible(!isAddRowVisible); 
  };

  const fetchTasks = async () => {
    const allTasks = await tasksApi.getTasks();
    setTasks(allTasks);
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-4xl bg-white p-6 rounded-lg shadow-lg">
        <div className='flex items-center justify-between'>
          <h1 className="text-3xl font-bold text-gray-800 mb-6">Task Manager</h1>
          <button className='mr-2 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 cursor-pointer' onClick={toggleAddRow}>{isAddRowVisible ? 'Cancel Add' : '+ Task'}</button>
        </div>
        <TaskList
          tasks={tasks}
          onUpdate={handleUpdateTask}
          onDelete={handleDeleteTask}
          onAdd={handleAddTask}
          showAddRow={isAddRowVisible} />
        <p className='text-red-400 text-center mt-2'>{errorText}</p>
      </div>
    </div>
  );
};

export default App;
