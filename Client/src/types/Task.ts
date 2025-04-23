export type TaskStatus = "To Do" | "In Progress" | "Done";

export interface Task {
  id: string;
  title: string;
  assignee: string;
  status: TaskStatus;
  lastModified: Date;
  version: any;
}
