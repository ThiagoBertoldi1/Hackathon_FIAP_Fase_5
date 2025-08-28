import { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Skeleton } from '@/components/ui/skeleton';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { CalendarDays, Video, MessageSquare, Activity } from 'lucide-react';

interface DataItem {
  id: string;
  jobId: string;
  videoName: string;
  message: string;
  status: 0 | 1 | 2 | 3;
  changedAtUtc: string;
}

interface QRCode {
  id: string;
  jobId: string;
  timestampSeconds: number,
  content: string,
  foundAtUtc: Date
}

const DataGrid = () => {
  const [data, setData] = useState<DataItem[]>([]);
  const [qrCodes, setQRCodes] = useState<QRCode[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedItem, setSelectedItem] = useState<DataItem | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const loadData = async () => {
    try {
      setLoading(true);
      const response = await fetch('http://localhost:5056/api/VideoChecker/get-all-jobs')
      if(!response.ok)
        setData([])
      
      setData(await response.json());
    } catch (error) {
    } finally {
      setLoading(false);
    }
  };

  const getQRCodes = async (jobId: string) => {
    try {
      const response = await fetch(`http://localhost:5056/api/VideoChecker/qrcodes-founds?id=${jobId}`)
      if(!response.ok)
        setQRCodes([])
      
      setQRCodes(await response.json());
    }
    catch (error) {
    }
  }

  useEffect(() => {
    loadData();
    
    const interval = setInterval(loadData, 5000);
    
    return () => clearInterval(interval);
  }, []);

  const getStatusColor = (status: number) => {
    switch (status) {
      case 0:
        return 'bg-yellow-500/20 text-yellow-400 border-yellow-500/50';
      case 1:
        return 'bg-blue-500/20 text-blue-400 border-blue-500/50';
      case 2:
        return 'bg-green-500/20 text-green-400 border-green-500/50';
      default:
        return 'bg-red-500/20 text-red-400 border-red-500/50';
    }
  };

  const getStatusText = (status: number) => {
    switch (status) {
      case 0:
        return 'Pendente';
      case 1:
        return 'Processando';
      case 2:
        return 'Processado';
      default:
        return 'Erro';
    }
  }

  const handleCardClick = (item: DataItem) => {
    setSelectedItem(item);
    if(item.status === 2)
      getQRCodes(item.jobId)
    setIsModalOpen(true);
  };

  if (loading && data.length === 0) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {Array.from({ length: 6 }).map((_, i) => (
          <Card key={i} className="border-border bg-card">
            <CardHeader>
              <Skeleton className="h-4 w-3/4" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-8 w-1/2 mb-2" />
              <Skeleton className="h-4 w-1/3" />
            </CardContent>
          </Card>
        ))}
      </div>
    );
  }

  return (
    <>
      <div className="grid grid-cols-1 gap-6">
        {data.map((item) => (
          <Card 
            key={item.id} 
            className="border-border bg-card hover:bg-accent/50 transition-all duration-300 hover:shadow-lg group cursor-pointer"
            onClick={() => handleCardClick(item)}
          >
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-card-foreground flex items-center gap-2">
                <Video className="h-4 w-4" />
                {item?.videoName ?? 'Vídeo sem nome'}
              </CardTitle>
              <Badge 
                variant="outline" 
                className={`text-xs ${getStatusColor(item.status)}`}
              >
                {getStatusText(item.status)}
              </Badge>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-primary group-hover:text-primary/80 transition-colors">
                {item.message}
              </div>
              <p className="text-xs text-muted-foreground mt-2 flex items-center gap-1">
                <CalendarDays className="h-3 w-3" />
                Atualizado: {item.changedAtUtc}
              </p>
            </CardContent>
          </Card>
        ))}
      </div>

      <Dialog open={isModalOpen} onOpenChange={setIsModalOpen}>
        <DialogContent className="sm:max-w-[600px] bg-card border-border">
          <DialogHeader>
            <DialogTitle className="text-card-foreground flex items-center gap-2">
              <Video className="h-5 w-5" />
              Detalhes do Job
            </DialogTitle>
          </DialogHeader>
          
          {selectedItem && (
            <div className="space-y-6">
              <div className="grid grid-cols-1 gap-4">
                <Card className="border-border bg-muted/50">
                  <CardHeader className="pb-3">
                    <CardTitle className="text-base flex items-center gap-2">
                      <Video className="h-4 w-4" />
                      Informações do Vídeo
                    </CardTitle>
                  </CardHeader>
                  <CardContent className="space-y-3">
                    <div>
                      <label className="text-sm font-medium text-muted-foreground">Nome do Vídeo</label>
                      <p className="text-sm text-card-foreground font-medium">
                        {selectedItem.videoName || 'Vídeo sem nome'}
                      </p>
                    </div>
                    <div>
                      <label className="text-sm font-medium text-muted-foreground">ID do Job</label>
                      <p className="text-sm text-card-foreground font-mono">
                        {selectedItem.id}
                      </p>
                    </div>
                  </CardContent>
                </Card>

                <Card className="border-border bg-muted/50">
                  <CardHeader className="pb-3">
                    <CardTitle className="text-base flex items-center gap-2">
                      <Activity className="h-4 w-4" />
                      Status e Progresso
                    </CardTitle>
                  </CardHeader>
                  <CardContent className="space-y-3">
                    <div>
                      <label className="text-sm font-medium text-muted-foreground">Status Atual</label>
                      <div className="mt-1">
                        <Badge 
                          variant="outline" 
                          className={`${getStatusColor(selectedItem.status)}`}
                        >
                          {getStatusText(selectedItem.status)}
                        </Badge>
                      </div>
                    </div>
                    <div>
                      <label className="text-sm font-medium text-muted-foreground">Última Atualização</label>
                      <p className="text-sm text-card-foreground flex items-center gap-1">
                        <CalendarDays className="h-3 w-3" />
                        {selectedItem.changedAtUtc}
                      </p>
                    </div>
                  </CardContent>
                </Card>

                {
                  selectedItem.status === 2 ? 
                    <Card className="border-border bg-muted/50">
                      <CardHeader className="pb-3">
                        <CardTitle className="text-base flex items-center gap-2">
                          <MessageSquare className="h-4 w-4" />
                          QRCodes encontrados
                        </CardTitle>
                      </CardHeader>
                      <CardContent>
                        <div className="bg-background/50 p-4 rounded-lg border border-border">
                          <p className="text-sm text-card-foreground leading-relaxed">
                            {qrCodes.map((item, i) => (<p>{i} - <a href={item.content} target="_blank">{item.content}</a></p>))}
                          </p>
                        </div>
                      </CardContent>
                    </Card> 
                  : ''
                }

                <Card className="border-border bg-muted/50">
                  <CardHeader className="pb-3">
                    <CardTitle className="text-base flex items-center gap-2">
                      <MessageSquare className="h-4 w-4" />
                      Mensagem do Sistema
                    </CardTitle>
                  </CardHeader>
                  <CardContent>
                    <div className="bg-background/50 p-4 rounded-lg border border-border">
                      <p className="text-sm text-card-foreground leading-relaxed">
                        {selectedItem.message}
                      </p>
                    </div>
                  </CardContent>
                </Card>
              </div>
            </div>
          )}
        </DialogContent>
      </Dialog>
    </>
  );
};

export default DataGrid;