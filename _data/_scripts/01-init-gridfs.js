const dbName = "filesdb";

db.getSiblingDB(dbName).createUser({
  user: 'appuser',
  pwd: 'apppass',
  roles: [{ role: "readWrite", db: dbName }],
});

const appdb = db.getSiblingDB(dbName);
appdb.createCollection("fs.files");
appdb.createCollection("fs.chunks");

appdb.fs.chunks.createIndex({ files_id: 1, n: 1 }, { unique: true });

appdb.createCollection("videos.files");
appdb.createCollection("videos.chunks");
appdb["videos.chunks"].createIndex({ files_id: 1, n: 1 }, { unique: true });
