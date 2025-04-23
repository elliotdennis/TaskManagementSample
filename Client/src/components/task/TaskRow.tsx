import { useState } from 'react';
import { Task } from '../../types/Task'; 
import { TaskStatusBadge } from './TaskStatusBadge';

type TaskRowProps = {
  task: Task;
  onUpdate: (task: Task) => void;
  onDelete: (id: string) => void;
};

export const TaskRow = ({ task, onUpdate, onDelete }: TaskRowProps) => {
  const [isEditing, setIsEditing] = useState(false);
  const [editedTask, setEditedTask] = useState({ ...task });

  const handleChange = (field: keyof Task, value: string) => {
    setEditedTask(prev => ({ ...prev, [field]: value }));
  };

  const handleSave = () => {
    onUpdate(editedTask);
    setIsEditing(false);
  };

  const handleCancel = () => {
    setEditedTask({ ...task });
    setIsEditing(false);
  };

  return (
    <tr className="even:bg-white odd:bg-gray-50 border-b border-gray-200">
      <td className="px-6 py-4">
        {isEditing ? (
          <input
            className="px-4 py-2 w-full border border-gray-300 rounded"
            value={editedTask.title}
            onChange={(e) => handleChange('title', e.target.value)}
          />
        ) : (
          task.title
        )}
      </td>
      <td className="px-6 py-4">
        {isEditing ? (
          <input
            className="px-4 py-2 w-full border border-gray-300 rounded"
            value={editedTask.assignee}
            onChange={(e) => handleChange('assignee', e.target.value)}
          />
        ) : (
          task.assignee
        )}
      </td>
      <td className="px-6 py-4">
        {isEditing ? (
          <select
            className="px-4 py-2 w-full border border-gray-300 rounded"
            value={editedTask.status}
            onChange={(e) => handleChange('status', e.target.value)}
          >
            <option value="To Do">To Do</option>
            <option value="In Progress">In Progress</option>
            <option value="Done">Done</option>
          </select>
        ) : (
          <TaskStatusBadge status={task.status} />
        )}
      </td>
      <td className="px-6 py-4 text-end">
        {isEditing ? (
          <>
            <button
              onClick={handleSave}
              className="bg-green-500 hover:bg-green-600 text-white px-3 py-1 rounded mr-2 cursor-pointer"
            >
              Save
            </button>
            <button
              onClick={handleCancel}
              className="bg-gray-400 hover:bg-gray-500 text-white px-3 py-1 rounded cursor-pointer"
            >
              Cancel
            </button>
          </>
        ) : (
          <>
            <button
              onClick={() => setIsEditing(true)}
              className="bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded mr-2 cursor-pointer"
            >
              Edit
            </button>
            <button
              onClick={() => onDelete(task.id)}
              className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded cursor-pointer"
            >
              Delete
            </button>
          </>
        )}
      </td>
    </tr>
  );
};
