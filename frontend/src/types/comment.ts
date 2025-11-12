export interface Comment {
  Id: string;
  TaskId: string;
  UserId: string;
  Content: string;
  CreatedAt: string;
  UserName: string;
}

export interface CreateCommentDto {
  TaskId: string;
  UserId: string;
  Content: string;
}

export interface PagedResult<T> {
  Items: T[];
  TotalCount: number;
  Page: number;
  PageSize: number;
  TotalPages: number;
  HasNextPage: boolean;
  HasPreviousPage: boolean;
}

export interface PaginationParams {
  page: number;
  pageSize: number;
}
