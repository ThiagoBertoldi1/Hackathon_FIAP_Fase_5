import { Button } from '@/components/ui/button';
import { Upload } from 'lucide-react';
import DataGrid from '@/components/DataGrid';
import VideoUploadModal from '@/components/VideoUploadModal';

const Index = () => {
  return (
    <div className="min-h-screen bg-background">
      {/* Hero Section */}
      <section className="relative overflow-hidden">
        <div className="absolute inset-0 bg-gradient-primary opacity-10" />
        <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-24">
          <div className="text-center">
            <h1 className="text-4xl md:text-6xl font-bold text-foreground mb-6">
              Video
              <span className="bg-gradient-primary bg-clip-text text-transparent"> Checker</span>
            </h1>
            <p className="text-xl text-muted-foreground mb-8 max-w-2xl mx-auto">
              Acompanhe informações em tempo real com atualizações automáticas a cada 5 segundos
            </p>
            
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <VideoUploadModal>
                <Button 
                  size="lg" 
                  className="bg-gradient-primary hover:opacity-90 shadow-glow transition-all duration-300"
                >
                  <Upload className="mr-2 h-5 w-5" />
                  Upload de Vídeo
                </Button>
              </VideoUploadModal>
            </div>
          </div>
        </div>
      </section>

      {/* Data Grid Section */}
      <section className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
        <div className="text-center mb-12">
          <h2 className="text-3xl font-bold text-foreground mb-4">
            Dados em Tempo Real
          </h2>
          <p className="text-muted-foreground">
            Acompanhe o processamento em tempo real
          </p>
        </div>
        
        <DataGrid />
      </section>

      {/* Footer */}
      <footer className="border-t border-border mt-16">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="text-center text-muted-foreground">
            <p>Um footer legal :D</p>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default Index;