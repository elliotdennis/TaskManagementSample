import { TaskStatus } from "../../types/Task";

export const TaskStatusBadge = ({ status }: { status: TaskStatus }) => {
    const colorMap = {
      'To Do': 'bg-blue-100 text-blue-800',
      'In Progress': 'bg-yellow-100 text-yellow-800',
      'Done': 'bg-green-100 text-green-800',
    };
  
    return (
      <span className={`px-2 py-1 rounded-full text-sm font-medium ${colorMap[status]}`}>
        {status}
      </span>
    );
  };
  