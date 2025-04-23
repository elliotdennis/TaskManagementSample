import { useState } from "react";
import { Task } from "../../types/Task";

type TaskAddRowProps = {
  onAdd: (task: Task) => void;
};

export const TaskAddRow = ({ onAdd }: TaskAddRowProps) => {
  const [newTask, setNewTask] = useState<Task>({
    id: "",
    title: "",
    assignee: "",
    status: "To Do",
    lastModified: new Date(),
    version: undefined,
  });

  const handleNewTaskChange = (field: string, value: string) => {
    setNewTask({ ...newTask, [field]: value });
  };

  const handleSave = () => {
    if (newTask.title && newTask.assignee) {
      onAdd(newTask);
      setNewTask({
        id: "",
        title: "",
        assignee: "",
        status: "To Do",
        lastModified: new Date(),
        version: undefined,
      });
    }
  };

  return (
    <tr className="border-b border-gray-200">
      <td className="px-6 py-4">
        <input
          type="text"
          value={newTask.title}
          onChange={(e) => handleNewTaskChange("title", e.target.value)}
          className="px-4 py-2 w-full border border-gray-300 rounded"
          placeholder="New Task Title"
        />
      </td>
      <td className="px-6 py-4">
        <input
          type="text"
          value={newTask.assignee}
          onChange={(e) => handleNewTaskChange("assignee", e.target.value)}
          className="px-4 py-2 w-full border border-gray-300 rounded"
          placeholder="Assignee"
        />
      </td>
      <td className="px-6 py-4">
        <select
          value={newTask.status}
          onChange={(e) => handleNewTaskChange("status", e.target.value)}
          className="px-4 py-2 w-full border border-gray-300 rounded"
        >
          <option value="To Do">To Do</option>
          <option value="In Progress">In Progress</option>
          <option value="Done">Done</option>
        </select>
      </td>
      <td className="px-6 py-4 text-end">
        <button
          onClick={handleSave}
          className="bg-green-500 hover:bg-green-600 text-white px-3 py-1 rounded mr-2 cursor-pointer"
        >
          Save
        </button>
      </td>
    </tr>
  );
};
