export const environment = {
  production: true,
  apiServer: 'https://localhost:5110',
  apis: () => {
    const server = 'https://localhost:5110';
    return {
      getFiles: `${server}/files`,
      upload: `${server}/upload`,
      getRows: `${server}/lastUploadedRows`,
    };
  },
};
