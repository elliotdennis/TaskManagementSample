import { Task } from '../../types/Task'; 
import { TaskRow } from './TaskRow';
import { TaskAddRow } from './TaskAddRow';

type TaskListProps = {
  tasks: Task[];
  onUpdate: (task: Task) => void;
  onDelete: (id: string) => void;
  onAdd: (task: Task) => void;
  showAddRow: boolean;
};

const TaskList: React.FC<TaskListProps> = ({
  tasks,
  onUpdate,
  onDelete,
  onAdd,
  showAddRow
}: TaskListProps) => {
  return (
    <div className="relative overflow-x-auto shadow-md sm:rounded-lg">
      <table className="w-full text-sm text-left rtl:text-right text-gray-500">
      <thead className="text-xs text-gray-700 uppercase border-b">
      <tr>
            <th className="px-6 py-3">Title</th>
            <th className="px-6 py-3">Assignee</th>
            <th className="px-6 py-3">Status</th>
            <th className="px-6 py-3 text-end">Actions</th>
          </tr>
        </thead>
              <tbody>
              {tasks.length === 0 && !showAddRow && (
            <tr className="border-b border-gray-200 text-center">
              <td className="px-6 py-4" colSpan={4}>
                No tasks to show
              </td>
            </tr>
          )}
          {tasks.map((task) => (
            <TaskRow
              key={task.id + task.lastModified}
              task={task}
              onUpdate={onUpdate}
              onDelete={onDelete}
            />
          ))}
          {showAddRow && (
            <TaskAddRow onAdd={onAdd} />
          )}
        </tbody>
      </table>
    </div>
  );
};

export default TaskList;
